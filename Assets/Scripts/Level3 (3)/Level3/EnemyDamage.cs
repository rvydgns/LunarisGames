using UnityEngine;
using ClearSky;

    
public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENEMY TRIGGER ENTER: " + other.name);

         if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(10, 0);
            }
            else
            {
                Debug.Log("PlayerHealth BULUNAMADI");
            }
        }
    }
}
