using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    [Header("UI Settings")]
    public TMP_Text collectableText;  // X/Total gösterilecek text

    [Header("Map Settings")]
    public int totalCollectablesInMap = 10;  // Bu mapte toplam kaç obje var

    [Header("Activation Objects")]
    public ActivationPair[] activationObjects;  // Hangi item hangi objeyi aktifleştirir

    private Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    private int totalCollected = 0;

    [System.Serializable]
    public class ActivationPair
    {
        public string itemID;  // Örn: "Skull", "Heart"
        public GameObject objectToActivate;  // Aktifleşecek obje
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateUI();
    }

    // Obje toplandığında çağrılacak
    public void CollectItem(string itemID, int amount = 1)
    {
        // Toplanan sayıyı arttır
        if (!collectedItems.ContainsKey(itemID))
        {
            collectedItems[itemID] = 0;
        }

        collectedItems[itemID] += amount;
        totalCollected += amount;

        Debug.Log($"{itemID} toplandı! Toplam: {collectedItems[itemID]}");

        // UI'ı güncelle
        UpdateUI();

        // İlgili objeyi aktifleştir
        ActivateObjectForItem(itemID);
    }

    // Belirli item ID'si için toplanan sayıyı al
    public int GetCollectedCount(string itemID)
    {
        return collectedItems.ContainsKey(itemID) ? collectedItems[itemID] : 0;
    }

    // Toplam toplanan sayıyı al
    public int GetTotalCollected()
    {
        return totalCollected;
    }

    private void UpdateUI()
    {
        if (collectableText != null)
        {
            collectableText.text = $"{totalCollected}/{totalCollectablesInMap}";
        }
    }

    private void ActivateObjectForItem(string itemID)
    {
        foreach (var pair in activationObjects)
        {
            if (pair.itemID == itemID && pair.objectToActivate != null)
            {
                pair.objectToActivate.SetActive(true);
                Debug.Log($"{itemID} için {pair.objectToActivate.name} aktifleştirildi!");
                break;
            }
        }
    }

    // Tüm verileri sıfırla (yeni map için)
    public void ResetCollection()
    {
        collectedItems.Clear();
        totalCollected = 0;
        UpdateUI();
    }
}
