using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour
{
    [Header("Toggle Object")]
    public GameObject targetObject;  

    
    public void ToggleObject()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object atanmadı!");
            return;
        }

        
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
