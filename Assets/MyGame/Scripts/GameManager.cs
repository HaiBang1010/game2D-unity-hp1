using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Nếu đã có một instance khác, hủy đối tượng này
            return;
        }

    }
    // Gọi khi thắng trận
    public void OnLevelComplete()
    {
        StartCoroutine(LoadNextLevelAfterDelay(SceneManager.GetActiveScene().buildIndex + 1, 3f));
    }

    private IEnumerator LoadNextLevelAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneIndex);
    }


}
