using UnityEngine;

public class DoorLevel4 : MonoBehaviour
{
    public Collider2D doorCollider;
    public GameObject doorVisual;

    public void OpenDoor()
    {
        doorCollider.enabled = false;
        doorVisual.SetActive(false);
    }
}
