using UnityEngine;
using TMPro;
using System.Collections;

public class HintCollectable : MonoBehaviour
{
    [Header("Hint Text")]
    [TextArea]
    public string hintMessage;

    [Header("UI Reference")]
    public TextMeshProUGUI hintText;

    [Header("Timing")]
    public float showDuration = 3f;
    public float freezeDuration = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (hintText == null)
        {
            Debug.LogError("❌ HintText atanmadı! Inspector'da bağla.", this);
            return;
        }

        // 🎯 PlayerMovement al
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Freeze(freezeDuration);
        }
        else
        {
            Debug.LogWarning("⚠ PlayerMovement bulunamadı!", this);
        }

        // 📝 Hint göster
        StartCoroutine(ShowHint());

        // 🧹 Collectable'ı sil
        Destroy(gameObject);
    }

    IEnumerator ShowHint()
    {
        hintText.text = hintMessage;
        hintText.gameObject.SetActive(true);

        yield return new WaitForSeconds(showDuration);

        hintText.gameObject.SetActive(false);
    }
}
