using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public LayerMask obstacleLayer;
    public float raycastDistance = 0.3f;

    public int maxHp = 10;
    private int currentHp;

    public Image healthBarFill; // 체력바 Fill 이미지

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        currentHp = maxHp;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;

        // 체력바 업데이트
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHp / maxHp;
        }

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // ✅ 총알에 맞았을 때 체력 감소
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1); // 총알 1개당 1 데미지

            Destroy(other.gameObject); // 총알 삭제
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 distance = player.position - transform.position;

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
            Vector3 scale = transform.localScale;
            scale.x = moveDirection.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (moveDirection == Vector2.zero) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, raycastDistance, obstacleLayer);

        Vector2 finalDirection = moveDirection;

        if (hit.collider != null)
        {
            finalDirection = Quaternion.Euler(0, 0, -90f) * moveDirection;
        }

        rb.MovePosition(rb.position + finalDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
