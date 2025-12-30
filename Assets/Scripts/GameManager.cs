using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // ==============================
    // Level 2 - Liora State
    // ==============================
    [Header("Level 2 - Liora State")]
    public bool lioraCutscenePlayed = false;
    public bool hasLioraHeal = false;

    [Header("Level 2 - Liora")]
    public GameObject lioraZoneTrigger;

    // ==============================
    // Collectibles
    // ==============================
    [Header("Collectibles")]
    public int collectedCount = 0;

    [Header("Level 2 - Reward")]
    public bool lioraRewardGiven = false;

    [Header("Boss Fight")]
    public bool isBossFightActive = false;


    // HANGİ ITEMLER ALINDI (asıl kritik kısım)
    private HashSet<ItemType> collectedItems = new HashSet<ItemType>();

    // ==============================
    // LIFECYCLE
    // ==============================
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ==============================
    // COLLECT ITEM
    // ==============================
    public void CollectItem(ItemType type)
    {
        // Aynı item ikinci kez alınamaz
        if (collectedItems.Contains(type))
            return;

        collectedItems.Add(type);
        collectedCount = collectedItems.Count;

        Debug.Log("Collected: " + type + " | Total Unique: " + collectedCount);

        // UI ikonunu aç
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowCollectible(type);
        }
        else
        {
            Debug.LogError("UIManager.instance is NULL");
        }

        // 3 FARKLI item alındıysa Liora trigger'ını aç
        if (collectedCount >= 3)
        {
            UnlockLioraTrigger();
        }
    }

    // ==============================
    // CHECK FUNCTIONS (ITEM SPAWN)
    // ==============================
    public bool IsItemCollected(ItemType type)
    {
        return collectedItems.Contains(type);
    }

    // ==============================
    // LIORA UNLOCK
    // ==============================
    private void UnlockLioraTrigger()
    {
        if (lioraZoneTrigger != null && !lioraZoneTrigger.activeSelf)
        {
            lioraZoneTrigger.SetActive(true);
            Debug.Log("Liora trigger unlocked");
        }
    }

    // ==============================
    // UI SYNC (SCENE LOAD)
    // ==============================
    public void SyncUIOnSceneLoad()
    {
        if (UIManager.instance == null) return;

        foreach (ItemType item in collectedItems)
        {
            UIManager.instance.ShowCollectible(item);
        }
    }

    public void GrantLioraHealPower()
    {
        // 1 kere verilsin
        if (lioraRewardGiven) return;

        // Güvenlik: 3 item tamamlanmadan verilmesin
        if (collectedCount < 3) return;

        hasLioraHeal = true;
        lioraRewardGiven = true;

        if (UIManager.instance != null)
            UIManager.instance.ShowLioraHealPowerUI();

        Debug.Log("Liora Heal Power granted (Heart +20).");
    }

}
