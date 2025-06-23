using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float maxTravelDistance = 10f;

    
    [Header("Damage")]
    public int damage = 1;

    private Vector2 direction;
    private Vector3 startPosition;

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
        if (Vector3.Distance(startPosition, transform.position) > maxTravelDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
