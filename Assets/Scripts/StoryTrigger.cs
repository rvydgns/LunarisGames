using UnityEngine;
using TMPro;
using System.Collections;

public class StoryTrigger : MonoBehaviour
{
    public GameObject storyPanel; 
    public TextMeshProUGUI storyText; 
    [TextArea] public string[] storyLines; 
    public float lineDisplayTime = 3f; 

    private int currentLine = 0;
    private CanvasGroup canvasGroup;
    private bool storyActive = false;

    private void Start()
    {
        
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
        
        yield return StartCoroutine(FadeIn());

        
        while (currentLine < storyLines.Length)
        {
            storyText.text = storyLines[currentLine];
            currentLine++;
            yield return new WaitForSeconds(lineDisplayTime);
        }

        
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

        
        PlayerMovement player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Freeze(0f);
        }
    }
}
