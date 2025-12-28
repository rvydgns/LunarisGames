using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneDataManager : MonoBehaviour
{
    private static SceneDataManager instance;
    
    // Her sahne için oyuncu pozisyonunu sakla
    private static Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
    // Önceki sahne adını sakla
    private static string previousScene = "";
    
    private void Awake()
    {
        // Singleton pattern
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

    // Mevcut sahne verilerini kaydet
    public static void SaveCurrentSceneData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            scenePlayerPositions[currentScene] = player.transform.position;
            
            // Önceki sahneyi kaydet
            previousScene = currentScene;
            
            Debug.Log($"{currentScene} sahnesi için pozisyon kaydedildi: {player.transform.position}");
        }
    }

    // Önceki sahne adını al
    public static string GetPreviousScene()
    {
        return previousScene;
    }

    // Sahne yüklendiğinde çağrılır
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Eğer bu sahne için kaydedilmiş pozisyon varsa, oyuncuyu oraya taşı
        if (scenePlayerPositions.ContainsKey(scene.name))
        {
            StartCoroutine(RestorePlayerPosition(scene.name));
        }
    }

    private System.Collections.IEnumerator RestorePlayerPosition(string sceneName)
    {
        // Oyuncunun spawn olması için 1 frame bekle
        yield return null;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && scenePlayerPositions.ContainsKey(sceneName))
        {
            Vector3 savedPosition = scenePlayerPositions[sceneName];
            player.transform.position = savedPosition;
            
            // Rigidbody varsa hızını sıfırla
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            
            Debug.Log($"{sceneName} sahnesi için pozisyon yüklendi: {savedPosition}");
        }
    }

    // Tüm kayıtları temizle (isteğe bağlı)
    public static void ClearAllData()
    {
        scenePlayerPositions.Clear();
        previousScene = "";
    }
}
