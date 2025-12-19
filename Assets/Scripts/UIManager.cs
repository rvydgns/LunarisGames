using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Collectibles")]
    public GameObject solariaIcon;
    public GameObject duskrootIcon;
    public GameObject periIcon;

    [Header("Powers")]
    public GameObject lioraHealIcon;
    [Header("Popups")]
    public GameObject newPowerPopup;
    public float popupDuration = 2.0f;

    private void Awake()
    {
        // ❗ UIManager SAHNEYE ÖZELDİR
        // ❗ Singleton çakışması / Destroy YOK
        instance = this;

        Debug.Log("UIManager READY (scene-local): " + gameObject.name);
    }

    private void Start()
    {
        // 🔒 Başlangıçta collectible ikonlarını kapat
        if (solariaIcon != null) solariaIcon.SetActive(false);
        if (duskrootIcon != null) duskrootIcon.SetActive(false);
        if (periIcon != null) periIcon.SetActive(false);

        // 🔒 Liora heal ikonu state'e göre ayarla
        if (lioraHealIcon != null)
        {
            if (GameManager.instance != null && GameManager.instance.hasLioraHeal)
                lioraHealIcon.SetActive(true);
            else
                lioraHealIcon.SetActive(false);
        }
    }

    // ==============================
    // COLLECTIBLES UI
    // ==============================
    public void ShowCollectible(ItemType type)
    {
        Debug.Log("UI ShowCollectible called: " + type);

        switch (type)
        {
            case ItemType.Solaria:
                if (solariaIcon != null)
                    solariaIcon.SetActive(true);
                else
                    Debug.LogError("Solaria icon reference missing!");
                break;

            case ItemType.Duskroot:
                if (duskrootIcon != null)
                    duskrootIcon.SetActive(true);
                else
                    Debug.LogError("Duskroot icon reference missing!");
                break;

            case ItemType.Peri:
                if (periIcon != null)
                    periIcon.SetActive(true);
                else
                    Debug.LogError("Peri icon reference missing!");
                break;
        }
    }

    // ==============================
    // LIORA HEAL POWER UI
    // ==============================
    public void ShowLioraHealPowerUI()
    {
        if (lioraHealIcon != null)
            lioraHealIcon.SetActive(true);
    }

    public void HideLioraHealPowerUI()
    {
        if (lioraHealIcon != null)
            lioraHealIcon.SetActive(false);
    }
    public void ShowNewPowerPopup()
    {
        if (newPowerPopup == null)
        {
            Debug.LogError("newPowerPopup is not assigned in UIManager!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(NewPowerPopupRoutine());
    }

    private System.Collections.IEnumerator NewPowerPopupRoutine()
    {
        newPowerPopup.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        newPowerPopup.SetActive(false);
    }

}
