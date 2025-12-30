using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CollectableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemID = "Skull";
    public int amount = 1;

    [Header("Floating Settings")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    [Header("Glow Settings")]
    public float glowSpeed = 2f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1f;

    private Vector3 startPos;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;

        // Güvenlik: collider trigger olsun
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Update()
    {
        // Yukarı - aşağı süzülme
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        // Parlama (alpha)
        float alpha = Mathf.Lerp(
            minAlpha,
            maxAlpha,
            (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f
        );

        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Toplandı: {itemID}");

            // İLERİDE BURAYA BAĞLAYACAĞIZ:
            // CollectableManager.Instance.Add(itemID, amount);

            Destroy(gameObject);
        }
    }
}
