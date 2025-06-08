using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 5f;

    [SerializeField] Sprite spriteUp;
    [SerializeField] Sprite spriteDown;
    [SerializeField] Sprite spriteLeft;
    [SerializeField] Sprite spriteRight;

    Rigidbody2D rb;
    SpriteRenderer sR;

    Vector2 input;
    Vector2 velocity;

    float score;

    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] float bulletSpeed = 5f;

    Transform nearestEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        velocity = input.normalized * moveSpeed;

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

        // 에임 타겟 찾기
        nearestEnemy = FindNearestEnemy();

        // Z키로 총알 발사
        if (Input.GetKeyDown(KeyCode.Z) && nearestEnemy != null)
        {
            ShootAtEnemy();
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
            score += collision.GetComponent<ItemObject>().GetPoint();
            scoreText.text = score.ToString();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Debug.Log("못지나감");
            velocity = Vector2.zero;
        }
    }

    // 가장 가까운 Enemy 찾기
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

    // 총알 발사 함수
    private void ShootAtEnemy()
    {
        Vector2 direction = (nearestEnemy.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.velocity = direction * bulletSpeed;
    }
}
