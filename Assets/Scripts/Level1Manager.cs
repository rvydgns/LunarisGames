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
        // Player otomatik bulunamazsa sahneden bul
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (player == null)
        {
            Debug.LogError("Level1Manager: PlayerMovement bulunamadı!");
            return;
        }

        // Orijinal hızı kaydet
        originalMoveSpeed = player.moveSpeed;

        // Level 1 için hızı düşür
        player.moveSpeed = level1MoveSpeed;

        Debug.Log("Level 1 speed applied: " + level1MoveSpeed);
    }

    // İstersen level bitince çağırırsın
    public void RestorePlayerSpeed()
    {
        if (player != null)
        {
            player.moveSpeed = originalMoveSpeed;
            Debug.Log("Player speed restored to: " + originalMoveSpeed);
        }
    }
}
