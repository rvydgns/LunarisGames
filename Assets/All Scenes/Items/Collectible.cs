using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemType itemType;
    private bool playerInRange = false;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    private void Collect()
    {
        Debug.Log("Collected: " + itemType);
        GameManager.instance.CollectItem(itemType);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        playerInRange = false;
    }
}
