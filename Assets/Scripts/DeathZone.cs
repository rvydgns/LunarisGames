using UnityEngine;

namespace ClearSky
{
    public class DeathZone : MonoBehaviour
    {
        public int damage = 10;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerHealth health = collision.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    health.TakeDeathZoneDamage(damage);
                }
            }
        }
    }
}
