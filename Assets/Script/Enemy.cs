using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float moveSpeed = 2f;             // 몬스터 이동 속도
    public float stopDistance = 0.5f;        // 플레이어와 너무 가까워지면 멈춤

    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        // 플레이어 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 몬스터의 Rigidbody2D 참조
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // 방향 계산
        Vector2 direction = (player.position - transform.position).normalized;

        // 플레이어와의 거리
        float distance = Vector2.Distance(transform.position, player.position);

        // 너무 가까우면 멈춤
        if (distance > stopDistance)
        {
            // 이동
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        // 스프라이트 반전 (좌우 방향)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
