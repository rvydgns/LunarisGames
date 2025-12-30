using UnityEngine;

public class Level4AudioManager : MonoBehaviour
{
    public AudioSource levelMusic;

    void Start()
    {
        StopAllOtherMusic();
        levelMusic.loop = true;
        levelMusic.Play();
    }

    void StopAllOtherMusic()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != levelMusic)
            {
                src.Stop();
            }
        }
    }
}
