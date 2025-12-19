using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;

    [Header("Player")]
    public PlayerMovement playerMovement;

    private bool isCutsceneActive = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        ClearTexts();
    }

    private void Update()
    {
        if (!isCutsceneActive) return;

        // ESC ile cutscene bitir (debug / skip)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndCutscene();
        }
    }

    // ==============================
    // START CUTSCENE
    // ==============================
    public void StartLioraCutscene()
    {
        if (isCutsceneActive) return;

        isCutsceneActive = true;

        // Player durdur
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Diyalog panelini aç
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // Sesleri cutscene moduna al
        if (Level2AudioController.instance != null)
            Level2AudioController.instance.EnterCutsceneAudio();
    }

    // ==============================
    // END CUTSCENE
    // ==============================
    public void EndCutscene()
    {
        if (!isCutsceneActive) return;

        isCutsceneActive = false;

        // Player hareketini geri aç
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Diyalog panelini kapat
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Sesleri normale döndür
        if (Level2AudioController.instance != null)
            Level2AudioController.instance.ExitCutsceneAudio();

        // Cutscene sonrası oyun mekaniği
        OnLioraCutsceneFinished();
    }

    // ==============================
    // CUTSCENE FINISHED LOGIC
    // ==============================
    private void OnLioraCutsceneFinished()
    {
        // Bir daha oynanmasın
        if (GameManager.instance != null)
            GameManager.instance.lioraCutscenePlayed = true;

        // Gücü ver
        if (GameManager.instance != null)
            GameManager.instance.hasLioraHeal = true;

        // UI göster
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowLioraHealPowerUI();
            UIManager.instance.ShowNewPowerPopup(); // popup UI
        }

        // Jingle çal
        if (AudioManager.instance != null)
            AudioManager.instance.PlayPowerUnlockedSFX();
    }

    private void ClearTexts()
    {
        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null) dialogueText.text = "";
    }
    
}
