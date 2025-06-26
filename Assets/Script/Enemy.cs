using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
   
    private SpriteRenderer sR;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public LayerMask obstacleLayer;
    public float raycastDistance = 0.3f;

    [Header("Health")]
    public int maxHp = 10;
    private int currentHp;
    public Image healthBarFill;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [Header("Drop Settings")]
    public GameObject expPrefab;

    void Awake()
    {
    
        sR = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (healthBarFill != null)
            healthBarFill.fillAmount = 1f;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (healthBarFill != null)
            healthBarFill.fillAmount = Mathf.Clamp01((float)currentHp / maxHp);
        if (currentHp <= 0) Die();
    }

    void Die()
    {
        if (expPrefab != null)
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어와 거리 계산
        Vector2 distance = player.position - transform.position;

        // 추적 범위 내면 단순 축 우선 이동 방향 결정
        if (distance.magnitude <= chaseRange)
        {
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                moveDirection = new Vector2(Mathf.Sign(distance.x), 0f);
            else
                moveDirection = new Vector2(0f, Mathf.Sign(distance.y));
        }
        else
        {
            moveDirection = Vector2.zero;
        }

        
        if (moveDirection.x != 0)
        {
       
       
            sR.flipX = moveDirection.x < 0;
        }
    }

    void FixedUpdate()
    {
        if (moveDirection == Vector2.zero) return;

        // 벽 회피용 레이캐스트
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, raycastDistance, obstacleLayer);
        Vector2 finalDirection = hit.collider != null
            ? (Vector2)(Quaternion.Euler(0, 0, -90f) * moveDirection)
            : moveDirection;

        rb.MovePosition(rb.position + finalDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
