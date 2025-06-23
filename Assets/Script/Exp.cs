
using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] private int expAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddExp(expAmount);
            }

            Destroy(gameObject);
        }
    }
}