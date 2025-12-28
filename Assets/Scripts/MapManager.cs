using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
   void Start()
{
    if (!PlayerPrefs.HasKey("LevelUnlocked_1"))
        PlayerPrefs.SetInt("LevelUnlocked_1", 1);

    Button[] buttons = FindObjectsOfType<Button>(true);

    foreach (Button btn in buttons)
    {
        string name = btn.gameObject.name;

        if (name.StartsWith("Level") && name.EndsWith("Button"))
        {
            int levelNumber = ExtractLevelNumber(name);
            bool unlocked = PlayerPrefs.GetInt("LevelUnlocked_" + levelNumber, 0) == 1;

            btn.interactable = unlocked;

            Image img = btn.GetComponent<Image>();

            if (unlocked)
            {
                img.color = Color.white; // mavi olmasın
                img.sprite = btn.spriteState.highlightedSprite != null
                    ? btn.spriteState.highlightedSprite
                    : img.sprite;
            }
            else
            {
                img.color = Color.white; // kritik nokta
                img.sprite = btn.spriteState.disabledSprite;
            }

            btn.onClick.RemoveAllListeners();
            if (unlocked)
            {
                btn.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("Level" + levelNumber + "Scene");
                });
            }
        }
    }
}

    int ExtractLevelNumber(string name)
    {
        string number = name.Replace("Level", "").Replace("Button", "");
        return int.Parse(number);
    }

    public static void UnlockNextLevel(int currentLevel)
    {
        int next = currentLevel + 1;
        PlayerPrefs.SetInt("LevelUnlocked_" + next, 1);
    }
}
