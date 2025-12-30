using UnityEngine;

public class PlayerRespawn5 : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (respawnPoint == null)
        {
            GameObject spawn = GameObject.Find("PlayerSpawn_L5");
            if (spawn != null)
                respawnPoint = spawn.transform;
        }
    }

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = respawnPoint.position;
    }

 
    public void SetCheckpoint(Transform newPoint)
    {
        respawnPoint = newPoint;
    }
}
