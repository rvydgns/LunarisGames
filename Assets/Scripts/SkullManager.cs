using UnityEngine;

public class SkullManager : MonoBehaviour
{
    public static SkullManager instance;
    public int collectedSkulls = 0;
    public int requiredSkulls = 5;

    private void Awake()
    {
        instance = this;
    }

    public void CollectSkull()
    {
        collectedSkulls++;

        Debug.Log("Toplanan skull: " + collectedSkulls);

        if (collectedSkulls >= requiredSkulls)
        {
            Debug.Log("Puzzle tamamlandı!");
            // BURAYA SONRA Aris açılacak
        }
    }
}

