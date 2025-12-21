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

    [Header("Flow")]
    public bool startHidden = true;
    public bool loadSceneWhenFinished = true;
    public string nextSceneName = "Level1";

    private int pageIndex = 0;

    private void Awake()
    {
        if (rootPanel == null) rootPanel = gameObject;
        if (startHidden) rootPanel.SetActive(false);
    }

    // Başlat butonuna bağlayacaksın
    public void Open()
    {  
        Debug.Log("OPEN CALLED");
        rootPanel.SetActive(true);
        pageIndex = 0;
        ShowPage();
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
            else
                rootPanel.SetActive(false);
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
