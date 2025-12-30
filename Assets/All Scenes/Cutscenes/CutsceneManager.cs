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

    
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndCutscene();
        }
    }

 
    public void StartLioraCutscene()
    {
        if (isCutsceneActive) return;

        isCutsceneActive = true;

    
        if (playerMovement != null)
            playerMovement.enabled = false;

   
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

 
        if (Level2AudioController.instance != null)
            Level2AudioController.instance.EnterCutsceneAudio();
    }

 
    public void EndCutscene()
    {
        if (!isCutsceneActive) return;

        isCutsceneActive = false;

        if (playerMovement != null)
            playerMovement.enabled = true;


        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

  
        if (Level2AudioController.instance != null)
            Level2AudioController.instance.ExitCutsceneAudio();

        OnLioraCutsceneFinished();
    }

 
    private void OnLioraCutsceneFinished()
    {
        
        if (GameManager.instance != null)
            GameManager.instance.lioraCutscenePlayed = true;

      
        if (GameManager.instance != null)
            GameManager.instance.hasLioraHeal = true;

       
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowLioraHealPowerUI();
            UIManager.instance.ShowNewPowerPopup(); 
        }

       
        if (AudioManager.instance != null)
            AudioManager.instance.PlayPowerUnlockedSFX();
    }

    private void ClearTexts()
    {
        if (speakerText != null) speakerText.text = "";
        if (dialogueText != null) dialogueText.text = "";
    }
    
}
