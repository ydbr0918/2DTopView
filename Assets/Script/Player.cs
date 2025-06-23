using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public int maxHp = 100;
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

    float fireRate = 0.5f;
    float lastShotTime = -999f;

    Vector2 lastMoveDirection = Vector2.down;
    Transform nearestEnemy;

    [Header("Target Marker")]
    [SerializeField] GameObject targetCirclePrefab;
    GameObject currentTargetCircle;
    Transform previousTarget;

    [Header("Weapon Data")]
    public WeaponData currentWeapon;

    [Header("Weapon Damage")]
    public int baseDamage = 10;

    bool isReloading = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();

        // UI 자동 할당 (씬에 정확한 이름 필요)
        healthSlider = healthSlider ?? GameObject.Find("PlayerHealthSlider")?.GetComponent<Slider>();
        healthText = healthText ?? GameObject.Find("PlayerHealthText")?.GetComponent<TextMeshProUGUI>();
        expText = expText ?? GameObject.Find("PlayerExpText")?.GetComponent<TextMeshProUGUI>();
        ammoText = ammoText ?? GameObject.Find("AmmoText")?.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // ----- 1) Health / Exp 초기화 -----
        currentHp = maxHp;
        UpdateHealthUI();
        UpdateExpUI();

        // ----- 2) Weapon 세팅 (무기 데이터 → maxAmmo/currentAmmo 세팅) -----
        currentWeapon = SelectionData.Instance.SelectedWeapon;
        if (currentWeapon != null)
        {
            fireRate = currentWeapon.fireRate;
            maxAmmo = currentWeapon.maxAmmo;
            currentAmmo = maxAmmo;
        }
        else
        {
            // 무기 미선택 시 경고
            Debug.LogWarning("[Player] SelectedWeapon 없음, 기본 무기 사용");
            currentAmmo = maxAmmo;
        }

        // ----- 3) Skill 적용 (무기 세팅 뒤에) -----
        var skill = SelectionData.Instance.SelectedSkill;
        if (skill != null)
        {
            skill.Activate(gameObject);
        }
        else
        {
            Debug.LogWarning("[Player] SelectedSkill 없음");
        }

        // 스킬이 maxAmmo를 늘려주었으면 currentAmmo 동기화
        currentAmmo = maxAmmo;

        // ----- 4) 초기 쿨다운 및 UI -----
        lastShotTime = Time.time - fireRate;
        UpdateAmmoUI();
    }

    private void Update()
    {
        // 이동 입력 & 스프라이트
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

        // 타겟 마커
        nearestEnemy = FindNearestEnemy();
        HandleTargetMarker();

        // 발사
        if (Input.GetKeyDown(KeyCode.Z) && !isReloading)
            TryShoot();

        // 수동 재장전
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
            StartCoroutine(Reload());

        // 스킬 재사용 (예: X키)
        if (Input.GetKeyDown(KeyCode.X))
            SelectionData.Instance.SelectedSkill?.Activate(gameObject);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void HandleTargetMarker()
    {
        if (nearestEnemy != previousTarget)
        {
            if (currentTargetCircle != null)
                Destroy(currentTargetCircle);

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
        if (Time.time - lastShotTime < fireRate) return;
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
        yield return new WaitForSeconds(1f);
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
    }

    void ShootWeapon()
    {
        Vector2 shootDir = nearestEnemy != null
            ? (nearestEnemy.position - transform.position).normalized
            : lastMoveDirection;

        int shotCount = currentWeapon?.bulletPerShot ?? 1;
        float spread = currentWeapon?.spreadAngle ?? 0f;
        float range = currentWeapon?.range ?? 10f;
        float speed = currentWeapon?.bulletSpeed ?? 5f;
        int weaponDmg = currentWeapon?.damage ?? 0;

        for (int i = 0; i < shotCount; i++)
        {
            float angle = Random.Range(-spread / 2f, spread / 2f);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * shootDir;
            var b = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            var bullet = b.GetComponent<Bullet>();
            bullet.SetDirection(dir);
            bullet.speed = speed;
            bullet.maxTravelDistance = range;

            
            bullet.damage = baseDamage + weaponDmg;
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
            TakeDamage(10);
        }
        else if (col.CompareTag("Ground"))
        {
            velocity = Vector2.zero;
        }
    }

    void TakeDamage(int amt)
    {
        currentHp = Mathf.Clamp(currentHp - amt, 0, maxHp);
        UpdateHealthUI();
        if (currentHp <= 0) Debug.Log("플레이어 사망");
    }

    public void AddExp(int amount)
    {
        exp += amount;
        while (exp >= expToNext)
        {
            exp -= expToNext;
            level++;
            expToNext += 5;
            maxHp += 10;
            currentHp = maxHp;
            UpdateHealthUI();
        }
        UpdateExpUI();
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
    {
        expText?.SetText($"{exp} / {expToNext}");
    }

    void UpdateAmmoUI()
    {
        ammoText?.SetText($"{currentAmmo} / {maxAmmo}");
    }

    /// <summary>HealSkill 에서 호출</summary>
    public void Heal(int amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
        UpdateHealthUI();
        Debug.Log($"[Player] Heal → +{amount} (now {currentHp}/{maxHp})");
    }

    /// <summary>AmmoBoostSkill 에서 호출</summary>
    public void IncreaseMaxAmmo(int amount)
    {
        maxAmmo += amount;
        currentAmmo += amount;
        UpdateAmmoUI();
        Debug.Log($"[Player] IncreaseMaxAmmo → +{amount}");
    }

    /// <summary>DamageBoostSkill 에서 호출</summary>
    public void IncreaseDamage(int amount)
    {
        baseDamage += amount;
        Debug.Log($"[Player] IncreaseDamage → +{amount} (now {baseDamage})");
    }
}

