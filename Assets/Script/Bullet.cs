using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;

    private Vector3 startPosition;
    public float maxTravelDistance = 10f; // 최대 거리

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // 시작 지점으로부터 일정 거리 이상 가면 삭제
        if (Vector3.Distance(startPosition, transform.position) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 좀비 스크립트에서 데미지 처리
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            // 적에게 닿았으면 총알 제거
            Destroy(gameObject);
        }
    }
}
