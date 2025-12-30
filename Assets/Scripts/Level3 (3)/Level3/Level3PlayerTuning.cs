using UnityEngine;

public class Level3PlayerTuning : MonoBehaviour
{
    public PlayerMovement movement;     
    [Range(0.1f, 1f)] public float jumpMultiplier = 0.5f;

    void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (movement == null) return;

        movement.jumpForce *= jumpMultiplier;  
    }
}
