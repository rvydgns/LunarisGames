using UnityEngine;

public class Level2AudioController : MonoBehaviour
{
    public static Level2AudioController instance;

    [Header("Ambient Sources")]
    public AudioSource forestSource;
    public AudioSource mysticSource;

    [Header("Volumes")]
    public float forestNormal = 0.35f;
    public float forestCutscene = 0.18f;

    public float mysticNormal = 0.30f;
    public float mysticCutscene = 0.20f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void EnterCutsceneAudio()
    {
        if (forestSource != null) forestSource.volume = Mathf.Min(forestSource.volume, forestCutscene);
        if (mysticSource != null) mysticSource.volume = Mathf.Min(mysticSource.volume, mysticCutscene);
    }

    public void ExitCutsceneAudio()
    {
     
        if (forestSource != null && forestSource.volume > 0f)
            forestSource.volume = forestNormal;

        if (mysticSource != null && mysticSource.volume > 0f)
            mysticSource.volume = mysticNormal;
    }
}
