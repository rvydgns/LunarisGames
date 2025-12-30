using UnityEngine;

public class BreakableGem5 : MonoBehaviour
{
    [Header("Break SFX")]
    public AudioClip breakSfx;
    [Range(0f, 1f)] public float breakSfxVolume = 0.8f;
    public int currentHits = 3;

    public void Break()
    {
        currentHits--;

        if (currentHits > 0)
            return;

        GemShakeCaller5.TriggerShake();

        if (breakSfx != null)
        {
            AudioSource.PlayClipAtPoint(
                breakSfx,
                transform.position,
                breakSfxVolume
            );
        }   


        if (BossMalvaren5.Instance != null)
            BossMalvaren5.Instance.TakeGemDamage(1);

        Destroy(gameObject);
    }
}
