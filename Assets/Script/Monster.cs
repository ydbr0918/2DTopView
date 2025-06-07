using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieFollow : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float directionChangeInterval = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;
    private float directionChangeTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        if (player == null) return;

        // Ÿ�̸� ī��Ʈ�ٿ�
        directionChangeTimer -= Time.deltaTime;

        if (directionChangeTimer <= 0f)
        {
            directionChangeTimer = directionChangeInterval;

            Vector2 distance = player.position - transform.position;

            if (distance.magnitude <= chaseRange)
            {
                // �밢�� �̵� ����: ���� ū �� �������θ� �̵�
                if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                {
                    moveDirection = new Vector2(Mathf.Sign(distance.x), 0f);
                }
                else
                {
                    moveDirection = new Vector2(0f, Mathf.Sign(distance.y));
                }

                //  ��������Ʈ ���� ó�� (�¿� �̵� ��)
                if (moveDirection.x != 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = moveDirection.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
            else
            {
                moveDirection = Vector2.zero;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
