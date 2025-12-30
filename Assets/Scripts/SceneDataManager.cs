using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneDataManager : MonoBehaviour
{
    private static SceneDataManager instance;
    
    
    private static Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
    
    private static string previousScene = "";
    
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

   
    public static void SaveCurrentSceneData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            scenePlayerPositions[currentScene] = player.transform.position;
            
           
            previousScene = currentScene;
            
            Debug.Log($"{currentScene} sahnesi için pozisyon kaydedildi: {player.transform.position}");
        }
    }

   
    public static string GetPreviousScene()
    {
        return previousScene;
    }

   
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scenePlayerPositions.ContainsKey(scene.name))
        {
            StartCoroutine(RestorePlayerPosition(scene.name));
        }
    }

    private System.Collections.IEnumerator RestorePlayerPosition(string sceneName)
    {
       
        yield return null;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && scenePlayerPositions.ContainsKey(sceneName))
        {
            Vector3 savedPosition = scenePlayerPositions[sceneName];
            player.transform.position = savedPosition;
            
            
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            
            Debug.Log($"{sceneName} sahnesi için pozisyon yüklendi: {savedPosition}");
        }
    }

    
    public static void ClearAllData()
    {
        scenePlayerPositions.Clear();
        previousScene = "";
    }
}
