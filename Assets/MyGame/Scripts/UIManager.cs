using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject heartPanel;
    public GameObject settingsButton;

    public GameObject mainMenu;
    public GameObject gameOverMenu;


    public Driver playerScript;
    public GameObject player;
    public GameObject invaders;
    public Invaders invadersScript;

    private bool isPaused = false;

    void Start()
    {
        int auto = PlayerPrefs.GetInt("AutoStart", 0);
        if (auto == 1)
        {
            PlayerPrefs.SetInt("AutoStart", 0);
            StartGame(); // gọi lại hàm như khi nhấn Play
        }
        else
        {
            Time.timeScale = 0f;
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
            settingsButton.SetActive(false);
            heartPanel.SetActive(false);
            gameOverMenu.SetActive(false);



            player.SetActive(false);
            invaders?.SetActive(false);
        }

    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        isPaused = !isPaused; // false

        pauseMenu.SetActive(isPaused); //flase
        settingsButton.SetActive(!isPaused);// doi lap voi true = false
        heartPanel.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ContinueGame()
    {
        TogglePause();
    }


    public void RestartGame()
    {
        PlayerPrefs.SetInt("AutoStart", 1);
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

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll(); // Xóa toàn bộ dữ liệu PlayerPrefs
        PlayerPrefs.Save();      // Đảm bảo ghi rằng đã xóa vào ổ đĩa
        Debug.Log("Đã giải phóng dữ liệu PlayerPrefs khi thoát game.");
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        heartPanel.SetActive(true);
        settingsButton.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;

        // bắt đầu animation bay lên
        player.SetActive(true); // bật máy bay
        invaders?.SetActive(true);
        invadersScript?.BeginInvaders();
        playerScript.StartIntroFlight(new Vector3(0f, -10f, 0f)); // vị trí bắt đầu ở gần đáy

        // Gọi phát âm thanh background
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("BackgoundAudio");
        }
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        heartPanel.SetActive(false);
        settingsButton.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);


    }
}
