using UnityEngine;
using TMPro;
using System.Collections;

public class StoryTrigger : MonoBehaviour
{
    public GameObject storyPanel; // Paneli inspector'dan atayabilirsin
    public TextMeshProUGUI storyText; // Panelin içindeki Text
    [TextArea] public string[] storyLines; // Yazılacak hikaye satırları
    public float lineDisplayTime = 3f; // Her satırın ekranda kalma süresi

    private int currentLine = 0;
    private CanvasGroup canvasGroup;
    private bool storyActive = false;

    private void Start()
    {
        // Panel başlangıçta kapalı ve şeffaf olsun
        canvasGroup = storyPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = storyPanel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        storyPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!storyActive && other.CompareTag("Player"))
        {
            storyActive = true;
            storyPanel.SetActive(true);
            currentLine = 0;
            storyText.text = storyLines[currentLine];

            // Player hareketini durdur
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Freeze(Mathf.Infinity);
            }

            StartCoroutine(PlayStory());
        }
    }

    private IEnumerator PlayStory()
    {
        // Fade-in panel
        yield return StartCoroutine(FadeIn());

        // Tüm satırları sırayla göster
        while (currentLine < storyLines.Length)
        {
            storyText.text = storyLines[currentLine];
            currentLine++;
            yield return new WaitForSeconds(lineDisplayTime);
        }

        // Fade-out panel ve player'ı serbest bırak
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        storyPanel.SetActive(false);
        storyActive = false;

        // Player hareketini tekrar aç
        PlayerMovement player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Freeze(0f);
        }
    }
}
