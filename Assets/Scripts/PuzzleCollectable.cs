using UnityEngine;

public class PuzzleCollectable : MonoBehaviour
{
    public CollectableType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        bool collected = PuzzleManager.Instance.TryCollect(type);

        if (collected)
        {
            Destroy(gameObject);
        }
        // yanlışsa hiçbir şey olmaz
    }
}
