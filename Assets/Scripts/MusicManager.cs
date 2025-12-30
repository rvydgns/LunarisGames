using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music Clips")]
    [SerializeField] private AudioClip startSceneMusic;
    [SerializeField] private AudioClip level1Music;

    private AudioSource src;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        src.loop = true;
        src.spatialBlend = 0f;

        
        PlayMusic(startSceneMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (src.clip == clip && src.isPlaying) return;

        src.clip = clip;
        src.Play();
    }

    public void StopMusic()
    {
        src.Stop();
        src.clip = null;
    }

    
    public void PlayStartMusic() => PlayMusic(startSceneMusic);
    public void PlayLevel1Music() => PlayMusic(level1Music);
}
