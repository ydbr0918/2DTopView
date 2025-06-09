using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed = 5f;
    Vector2 input;
    Vector2 velocity;
    Rigidbody2D rb;
    SpriteRenderer sR;

    [Header("Sprites")]
    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    [Header("Score")]
    float score;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed = 5f;
    Transform nearestEnemy;

    [Header("Health")]
    public int maxHp = 100;
    private int currentHp;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Leveling")]
    private int exp = 0;
    private int level = 1;
    private int expToNext = 10;

    [SerializeField] TextMeshProUGUI expText;

    [Header("Ammo")]
    [SerializeField] int maxAmmo = 10;
    private int currentAmmo;
    [SerializeField] float fireRate = 0.5f;
    private float lastShotTime = -999f;

    [SerializeField] TextMeshProUGUI ammoText;

    private Vector2 lastMoveDirection = Vector2.down;





    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHp = maxHp;
        UpdateHealthUI();
        UpdateExpUI();
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;

    }

    private bool isReloading = false;

    private void Update()
    {
        // 이동 입력
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        velocity = input.normalized * moveSpeed;

        // 방향에 따라 스프라이트 전환
        if (input.sqrMagnitude > .01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                sR.sprite = input.x > 0 ? spriteRight : spriteLeft;
            }
            else
            {
                sR.sprite = input.y > 0 ? spriteUp : spriteDown;
            }
        }

        // 가장 가까운 적 찾기
        nearestEnemy = FindNearestEnemy();

        if (input.sqrMagnitude > .01f)
        {
            lastMoveDirection = input.normalized; // 최근 이동 방향 저장

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                sR.sprite = input.x > 0 ? spriteRight : spriteLeft;
            }
            else
            {
                sR.sprite = input.y > 0 ? spriteUp : spriteDown;
            }
        }

        // Z 키로 총알 발사
        if (Input.GetKeyDown(KeyCode.Z) && !isReloading)
        {
            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (Time.time - lastShotTime >= fireRate)
            {
                ShootAtEnemy();
                currentAmmo--;
                UpdateAmmoUI();
                lastShotTime = Time.time;

                if (currentAmmo <= 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }



        // 수동 재장전
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

    }


    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(1f);

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        Debug.Log("재장전 완료");
        isReloading = false;
    }


    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }


    public void AddExp(int amount)
    {
        exp += amount;

        while (exp >= expToNext)
        {
            exp -= expToNext;
            level++;
            expToNext += 5; // 다음 레벨 요구 경험치 +5
            OnLevelUp();
        }

        UpdateExpUI();
    }



    private void OnLevelUp()
    {
        Debug.Log($"레벨업! Lv {level}");

        maxHp += 10;
        currentHp = maxHp;

        UpdateHealthUI();
    }

    private void UpdateExpUI()
    {
        if (expText != null)
        {
            expText.text = $"{exp} / {expToNext}";
        }
    }



    


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Item"))
    {
        // ItemObject가 null이 아닐 때만 처리
        var itemObj = collision.GetComponent<ItemObject>();
        if (itemObj != null)
        {
            score += itemObj.GetPoint();
            if (scoreText != null)
                scoreText.text = score.ToString();
            Destroy(collision.gameObject);
        }
    }

    if (collision.CompareTag("Ground"))
    {
        Debug.Log("못지나감");
        velocity = Vector2.zero;
    }

    if (collision.CompareTag("Enemy"))
    {
        TakeDamage(10);
    }
}


    private void TakeDamage(int amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHealthUI();

        if (currentHp <= 0)
        {
            Debug.Log("플레이어 사망");
            
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = currentHp;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHp} / {maxHp}";
        }
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    private void ShootAtEnemy()
    {
        Vector2 direction;

        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.position - transform.position).normalized;
        }
        else
        {
            direction = lastMoveDirection;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.velocity = direction * bulletSpeed;
    }


}

