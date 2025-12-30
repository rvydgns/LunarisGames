using System.Collections;
using UnityEngine;

public class AmbientZoneSwitcher : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource forestSource;
    public AudioSource mysticSource;

    [Header("Volumes")]
    public float forestNormal = 0.35f;
    public float mysticNormal = 0.30f;

    [Header("Fade")]
    public float fadeDuration = 2.0f;

    private Coroutine fadeRoutine;

    private void Start()
    {
        // Güvenli başlangıç
        if (forestSource != null) forestSource.volume = forestNormal;
        if (mysticSource != null) mysticSource.volume = 0f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        StartFade(toMystic: true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        StartFade(toMystic: false);
    }

    private void StartFade(bool toMystic)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(toMystic));
    }

    private IEnumerator FadeRoutine(bool toMystic)
    {
        float t = 0f;

        float forestStart = forestSource != null ? forestSource.volume : 0f;
        float mysticStart = mysticSource != null ? mysticSource.volume : 0f;

        float forestTarget = toMystic ? 0f : forestNormal;
        float mysticTarget = toMystic ? mysticNormal : 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);

            if (forestSource != null) forestSource.volume = Mathf.Lerp(forestStart, forestTarget, k);
            if (mysticSource != null) mysticSource.volume = Mathf.Lerp(mysticStart, mysticTarget, k);

            yield return null;
        }

        if (forestSource != null) forestSource.volume = forestTarget;
        if (mysticSource != null) mysticSource.volume = mysticTarget;
    }
}
