using UnityEngine;

public class GameManagerLevel4 : MonoBehaviour
{
    public static GameManagerLevel4 Instance;

    [Header("Collectable Settings")]
    public int totalCollectables = 26;
    private int collectedCount = 0;

    [Header("Door")]
    public DoorLevel4 door;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectItem()
    {
        collectedCount++;

        if (collectedCount >= totalCollectables)
        {
            door.OpenDoor();
        }
    }
}
