using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryPopupController : MonoBehaviour
{
    [Header("Data")]
    public StoryData storyData;

    [Header("UI References")]
    public GameObject rootPanel;     // popup'ın en üst objesi
    public TMP_Text storyText;
    public TMP_Text continueText;

    [Header("Objects to Deactivate")]
    public GameObject[] objectsToDeactivate;

    [Header("Panel Sequence (Left Click)")]
    public GameObject[] panelSequence;  // Inspector'dan eklenecek paneller

    [Header("Flow")]
    public bool startHidden = true;
    public bool loadSceneWhenFinished = true;
    public string nextSceneName = "Level1";

    private int pageIndex = 0;
    private int panelIndex = 0;  // Hangi panel aktif

    private void Awake()
    {
        if (rootPanel == null) rootPanel = gameObject;
        if (startHidden) rootPanel.SetActive(false);
    }

    private void Update()
    {
        // rootPanel aktifken sol tıkla paneller arasında geçiş yap
        if (rootPanel != null && rootPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            OnLeftClick();
        }
    }

    // Başlat butonuna bağlayacaksın
    public void Open()
    {
        // Belirtilen objeleri deaktif et
        if (objectsToDeactivate != null && objectsToDeactivate.Length > 0)
        {
            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
        
        rootPanel.SetActive(true);
        pageIndex = 0;
        panelIndex = 0;
        ShowPage();
        ShowPanelAtIndex(0);
    }

    // Sol tık ile otomatik çağrılacak
    private void OnLeftClick()
    {
        if (panelSequence == null || panelSequence.Length == 0) return;

        panelIndex++;
        
        if (panelIndex < panelSequence.Length)
        {
            ShowPanelAtIndex(panelIndex);
        }
        else
        {
            // Tüm paneller bitti, bir sonraki tıkta sahneye geç
            if (loadSceneWhenFinished)
                SceneManager.LoadScene(nextSceneName);
        }
    }

    private void ShowPanelAtIndex(int index)
    {
        if (panelSequence == null || panelSequence.Length == 0) return;
        
        for (int i = 0; i < panelSequence.Length; i++)
        {
            if (panelSequence[i] != null)
            {
                panelSequence[i].SetActive(i == index);
            }
        }
    }

    // Parşömene tıklayınca çağrılacak
    public void NextPage()
    {
        if (storyData == null || storyData.pages == null || storyData.pages.Length == 0) return;

        if (pageIndex < storyData.pages.Length - 1)
        {
            pageIndex++;
            ShowPage();
        }
        else
        {
            if (loadSceneWhenFinished)
                SceneManager.LoadScene(nextSceneName);
        }
    }

    private void ShowPage()
    {
        
        storyText.text = storyData.pages[pageIndex];
        continueText.text = (pageIndex < storyData.pages.Length - 1)
            ? "Devam etmek için tıkla"
            : "Başlamak için tıkla";
    }
}
