using UnityEngine;

public class AutoDestroy5 : MonoBehaviour
{
    public float lifeTime = 0.6f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
