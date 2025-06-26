using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    Vector2 input, velocity;
    Rigidbody2D rb;
    SpriteRenderer sR;

    [Header("Sprites")]
    [SerializeField] Sprite spriteUp, spriteDown, spriteLeft, spriteRight;

    [Header("Score")]
    float score;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;

    [Header("Health")]
    [SerializeField] int maxHp = 100;
    int currentHp;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Level")]
    int exp = 0, level = 1, expToNext = 10;
    [SerializeField] TextMeshProUGUI expText;

    [Header("Ammo/UI")]
    [SerializeField] int maxAmmo = 10;
    int currentAmmo;
    [SerializeField] TextMeshProUGUI ammoText;

    public int Level => level;
    public int Exp => exp;
    public int ExpToNext => expToNext;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int MaxAmmo => maxAmmo;
    public int CurrentAmmo => currentAmmo;

    [Header("Weapon Data")]
    public WeaponData weaponData;
    private WeaponData currentWeapon;
   
  
    private float runtimeFireRate;
    private float runtimeBulletSpeed;
    int runtimeBaseDamage;

    public float FireRate => runtimeFireRate;
    public float BulletSpeed => runtimeBulletSpeed;
    public int BaseDamage => runtimeBaseDamage;

    bool isReloading = false;
    float lastShotTime = -999f;

    Vector2 lastMoveDirection = Vector2.down;
    Transform nearestEnemy;

    [Header("Target Marker")]
    [SerializeField] GameObject targetCirclePrefab;
    GameObject currentTargetCircle;
    Transform previousTarget;

    [Header("Level Up UI")]
    [SerializeField] TextMeshProUGUI levelUpText;
    Coroutine levelUpCoroutine;

    InGameUIManager uiManager;

    


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();

        uiManager = FindObjectOfType<InGameUIManager>();
        if (levelUpText == null)
        {
            var go = GameObject.Find("LevelUpText");
            if (go != null)
                levelUpText = go.GetComponent<TextMeshProUGUI>();
            else
                Debug.LogWarning("[Player] LevelUpText 오브젝트를 찾을 수 없습니다!");
        }

        if (levelUpText != null)
            levelUpText.gameObject.SetActive(false);
        
        healthSlider = healthSlider ?? GameObject.Find("PlayerHealthSlider")?.GetComponent<Slider>();
        healthText = healthText ?? GameObject.Find("PlayerHealthText")?.GetComponent<TextMeshProUGUI>();
        expText = expText ?? GameObject.Find("PlayerExpText")?.GetComponent<TextMeshProUGUI>();
        ammoText = ammoText ?? GameObject.Find("AmmoText")?.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (SaveManager.HasSave)
            LoadSavedData();
        else
            InitNewGame();
    }

    private void InitNewGame()
    {
        // 기본 초기화
        currentHp = maxHp;
        exp = 0;
        expToNext = 10;
        currentAmmo = maxAmmo;
        UpdateHealthUI();
        UpdateExpUI();
        UpdateAmmoUI();

        // ▶무기/스킬 선택값 가져오기
        weaponData = SelectionData.Instance.SelectedWeapon;
        //무기 원본 수치
        float weaponFireRate = weaponData != null ? weaponData.fireRate : 0.5f;
        float weaponBulletSpeed = weaponData != null ? weaponData.bulletSpeed : 5f;
        int weaponBaseDamage = weaponData != null ? weaponData.damage : 10;

        //런타임 변수에 복사
        runtimeFireRate = weaponFireRate;
        runtimeBulletSpeed = weaponBulletSpeed;
        runtimeBaseDamage = weaponBaseDamage;

        // 스킬 버프
        SelectionData.Instance.SelectedSkill?.Activate(gameObject);

        // UI / 쿨다운 동기화
        UpdateAmmoUI();
        lastShotTime = Time.time - runtimeFireRate;
    }


    private void LoadSavedData()
    {
        SaveManager.LoadGame();
        var d = SaveManager.LoadedData;

        // 저장된 HP복원
        level = d.level;
        exp = d.exp;
        maxHp = d.maxHp;
        currentHp = d.currentHp;
        maxAmmo = d.maxAmmo;
        currentAmmo = d.currentAmmo;

        // 복원
        runtimeFireRate = d.savedFireRate;
        runtimeBulletSpeed = d.savedBulletSpeed;
        runtimeBaseDamage = d.savedBaseDamage;

        
        UpdateHealthUI();
        UpdateExpUI();
        UpdateAmmoUI();

   
    }
    void Update()
    {
        // 이동
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        velocity = input.normalized * moveSpeed;
        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                sR.sprite = input.x > 0 ? spriteRight : spriteLeft;
            else
                sR.sprite = input.y > 0 ? spriteUp : spriteDown;
            lastMoveDirection = input.normalized;
        }

        // 타겟
        nearestEnemy = FindNearestEnemy();
        HandleTargetMarker();

        // 사격
        if (Input.GetKeyDown(KeyCode.Z) && !isReloading) TryShoot();
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
            StartCoroutine(Reload());
        if (Input.GetKeyDown(KeyCode.P))
            SelectionData.Instance.SelectedSkill?.Activate(gameObject);
    }

    void FixedUpdate()
        => rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

    void HandleTargetMarker()
    {
        if (nearestEnemy != previousTarget)
        {
            if (currentTargetCircle != null) Destroy(currentTargetCircle);
            if (nearestEnemy != null)
            {
                currentTargetCircle = Instantiate(
                    targetCirclePrefab,
                    nearestEnemy.position + Vector3.down * 0.4f,
                    Quaternion.identity,
                    nearestEnemy
                );
                currentTargetCircle.transform.localPosition = Vector3.down * 0.1f;
            }
            previousTarget = nearestEnemy;
        }
    }

    void TryShoot()
    {
        if (Time.time - lastShotTime < runtimeFireRate) return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        ShootWeapon();
        lastShotTime = Time.time;
        currentAmmo--;
        UpdateAmmoUI();
        if (currentAmmo <= 0)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        uiManager?.ShowReloadText();
        yield return new WaitForSeconds(runtimeFireRate);
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
        uiManager?.HideReloadText();
    }

    private void ShootWeapon()
    {
        
        Vector2 shootDir = nearestEnemy != null
            ? (nearestEnemy.position - transform.position).normalized
            : lastMoveDirection;

       
        int shotCount = currentWeapon != null ? currentWeapon.bulletPerShot : 1;
        float spreadAngle = currentWeapon != null ? currentWeapon.spreadAngle : 0f;
        float range = currentWeapon != null ? currentWeapon.range : 10f;

       
        for (int i = 0; i < shotCount; i++)
        {
            float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * shootDir;

            var go = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            var bullet = go.GetComponent<Bullet>();
            if (bullet == null) continue;

         
            bullet.SetDirection(dir);
            bullet.speed = runtimeBulletSpeed;  
            bullet.maxTravelDistance = range;
            bullet.damage = runtimeBaseDamage;   
        }
    }



    Transform FindNearestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minD = float.MaxValue;
        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minD) { minD = d; closest = e.transform; }
        }
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            var item = col.GetComponent<ItemObject>();
            if (item != null)
            {
                score += item.GetPoint();
                scoreText?.SetText(score.ToString());
            }
            Destroy(col.gameObject);
        }
        else if (col.CompareTag("Enemy"))
        {
            currentHp = Mathf.Clamp(currentHp - 10, 0, maxHp);
            UpdateHealthUI();
        }
    }

    public void AddExp(int amount)
    {
        exp += amount;
        bool didLevelUp = false;

        while (exp >= expToNext)
        {
            exp -= expToNext;
            level++;
            expToNext += 5;
            maxHp += 10;
            currentHp = maxHp;
            UpdateHealthUI();
            didLevelUp = true;
        }
        UpdateExpUI();

        if (didLevelUp)
            ShowLevelUpText();  
    }




    private void ShowLevelUpText()
    {
        if (levelUpCoroutine != null)
            StopCoroutine(levelUpCoroutine);
        levelUpCoroutine = StartCoroutine(LevelUpCoroutine());
    }

    private IEnumerator LevelUpCoroutine()
    {
        levelUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        levelUpText.gameObject.SetActive(false);
    }


    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHp;
        }
        healthText?.SetText($"{currentHp} / {maxHp}");
    }

    void UpdateExpUI()
        => expText?.SetText($"{exp} / {expToNext}");

    void UpdateAmmoUI()
        => ammoText?.SetText($"{currentAmmo} / {maxAmmo}");

    public void IncreaseDamage(int amount)
    {
     
        runtimeBaseDamage += amount;
        Debug.Log($"[Player] DamageBoostSkill 적용 → +{amount}, 현재 베이스 데미지 = {runtimeBaseDamage}");
    }

    
    public void IncreaseMaxAmmo(int amount)
    {
        maxAmmo += amount;
        currentAmmo += amount;
        UpdateAmmoUI();
        Debug.Log($"[Player] AmmoBoostSkill 적용 → +{amount}, 현재 탄창 크기 = {maxAmmo}");
    }
}
