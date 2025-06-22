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

    public Image healthBarFill;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [Header("Drop Settings")]
    public GameObject expPrefab; 

    void Start()
    {
        currentHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp01((float)currentHp / maxHp);
        }

        if (currentHp <= 0)
        {
            Die(); 
        }
    }

    void Die()
    {
        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }

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

        if (healthBarFill != null)
        {
           
            healthBarFill.rectTransform.localScale = new Vector3(1, 1, 1);
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
