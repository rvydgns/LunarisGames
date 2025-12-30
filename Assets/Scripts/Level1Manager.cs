using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerMovement player;

    [Header("Speed Settings")]
    [Tooltip("Level 1'de oyuncu hızı")]
    public float level1MoveSpeed = 3f;

    private float originalMoveSpeed;

    void Start()
    {
        
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (player == null)
        {
            Debug.LogError("Level1Manager: PlayerMovement bulunamadı!");
            return;
        }

        
        originalMoveSpeed = player.moveSpeed;

      
        player.moveSpeed = level1MoveSpeed;

        Debug.Log("Level 1 speed applied: " + level1MoveSpeed);
    }

   
    public void RestorePlayerSpeed()
    {
        if (player != null)
        {
            player.moveSpeed = originalMoveSpeed;
            Debug.Log("Player speed restored to: " + originalMoveSpeed);
        }
    }
}
