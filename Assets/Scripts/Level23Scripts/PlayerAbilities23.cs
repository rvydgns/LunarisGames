using UnityEngine;
using ClearSky;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        HandleLioraHeal();
    }

    void HandleLioraHeal()
    {
        // Level3 gibi sahnelerde GameManager yoksa patlamasın
        if (GameManager.instance == null)
            return;

        if (!GameManager.instance.hasLioraHeal)
            return;

        // PlayerHealth daha başlamadıysa (edge-case) patlamasın
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth.Heal(20);
            GameManager.instance.hasLioraHeal = false;

            Debug.Log("Liora'nın Şifası kullanıldı (+20 HP)");
        }
    }

}
