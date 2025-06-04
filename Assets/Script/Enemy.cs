using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float moveSpeed = 2f;             // ���� �̵� �ӵ�
    public float stopDistance = 0.5f;        // �÷��̾�� �ʹ� ��������� ����

    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        // �÷��̾� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ������ Rigidbody2D ����
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // ���� ���
        Vector2 direction = (player.position - transform.position).normalized;

        // �÷��̾���� �Ÿ�
        float distance = Vector2.Distance(transform.position, player.position);

        // �ʹ� ������ ����
        if (distance > stopDistance)
        {
            // �̵�
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        // ��������Ʈ ���� (�¿� ����)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
