using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject heartPanel;
    public GameObject settingsButton;
    public GameObject mainMenu;

    private bool isPaused = false;

    void Start()
    {
        // Bắt đầu game ở trạng thái dừng, hiển thị Main Menu
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        settingsButton.SetActive(false);
        heartPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // isPaused = true; isPaused = false;

        pauseMenu.SetActive(isPaused);
        settingsButton.SetActive(!isPaused);
        heartPanel.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ContinueGame()
    {
        TogglePause();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

     public void StartGame()
    {
        mainMenu.SetActive(false);
        heartPanel.SetActive(true);
        settingsButton.SetActive(true);
        Time.timeScale = 1f;
    }
}
