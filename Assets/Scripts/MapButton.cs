using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour
{
    [Header("Toggle Object")]
    public GameObject targetObject;  // Inspector'dan aktif/deaktif edilecek obje

    // Butona bağlanacak metot
    public void ToggleObject()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object atanmadı!");
            return;
        }

        // Objeyi aktif/deaktif et
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
