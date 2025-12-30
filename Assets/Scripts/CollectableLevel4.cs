using UnityEngine;

public class CollectableLevel4 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerLevel4.Instance.CollectItem();
            Destroy(gameObject);
        }
    }
}
