using UnityEngine;

public class CollectSkull : MonoBehaviour
{
    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;
        ObjectiveTracker.Instance.Add(ObjectiveId.SkullCollect, 1);
        Destroy(gameObject);
    }
}
