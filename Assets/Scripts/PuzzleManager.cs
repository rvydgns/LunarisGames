using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [Header("Puzzle State")]
    private int step = 0;
    // 0 = hiçbir şey
    // 1 = silah
    // 2 = kitap
    // 3 = elmas → tamamlandı

    [Header("References")]
    public GameObject gateObject;
    public TMP_Text warningText;

    private void Awake()
    {
        // 🔒 Singleton güvenliği
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    public bool TryCollect(CollectableType type)
    {
        // ✅ DOĞRU SIRA
        if (step == 0 && type == CollectableType.Weapon)
        {
            step = 1;
            return true;
        }

        if (step == 1 && type == CollectableType.Book)
        {
            step = 2;
            return true;
        }

        if (step == 2 && type == CollectableType.Diamond)
        {
            step = 3;
            PuzzleCompleted();
            return true;
        }

        // ❌ YANLIŞ SIRA
        ShowWarning();
        return false;
    }

    void PuzzleCompleted()
    {
        Debug.Log("🎉 Puzzle tamamlandı!");

        if (gateObject != null)
            gateObject.SetActive(false); // 🚪 Alan açılır
    }

    void ShowWarning()
    {
        if (warningText == null) return;

        warningText.text = "Bu doğru yol değil.";
        warningText.gameObject.SetActive(true);

        CancelInvoke();
        Invoke(nameof(HideWarning), 2f);
    }

    void HideWarning()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }
}
