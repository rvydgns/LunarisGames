using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("UI / Event SFX")]
    public AudioSource uiSfxSource;
    public AudioClip powerUnlockedSFX;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

   
    public void PlayPowerUnlockedSFX()
    {
        if (uiSfxSource != null && powerUnlockedSFX != null)
        {
            uiSfxSource.PlayOneShot(powerUnlockedSFX);
        }
    }
}
