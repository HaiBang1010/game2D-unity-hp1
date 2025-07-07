using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;


// dùng để điều khiển nhân vật máy bay của bản thân
public class Driver : MonoBehaviour
{
    private bool isStarting = false;
    private Vector3 startTarget;
    private UIManager uiManager; // hiển thị màn hình game over
    [SerializeField] float moveSpeed = 10f; // tốc độ di chuyển của nhân vật
    public Projectile laserPrefab; // tên lửa
    // Số mạng của người chơi
    public int lives = 3; // số mạng của người chơi
    public Image[] hearts; // mạng sống của người chơi

    //làm nhấp nháy khi bị trúng đạn
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f; // thời gian bất tử sau khi trúng đòn
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in scene!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarting)
        {
            StartCoroutine(StartMoveAfterDelay(2f));
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startTarget, step);

            if (Vector3.Distance(transform.position, startTarget) < 0.01f)
            {
                isStarting = false; // kết thúc intro
            }
            return; // chưa cho phép điều khiển khi chưa lên tới nơi
        }

        //di chuyển động của player
        float changeSteerH = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float changeSteerV = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(changeSteerH, changeSteerV, 0);


        // Giới hạn vị trí theo tọa độ thế giới + padding
        Vector3 pos = transform.position;

        // Lấy giới hạn từ Camera
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Padding để điều chỉnh khoảng cách viền
        float paddingX = 1f;
        float paddingY = 1f;

        pos.x = Mathf.Clamp(pos.x, -cameraWidth + paddingX, cameraWidth - paddingX);
        pos.y = Mathf.Clamp(pos.y, -cameraHeight + paddingY, cameraHeight - paddingY);

        transform.position = pos;
        // bắn tên lửa khi nhấn phím space hoặc click chuột
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            FireLaser();
        }
    }

    // Hàm bắn tên lửa
    private void FireLaser()
    {
        Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, this.laserPrefab.transform.rotation);
        // Gọi phát âm thanh bắn laser
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ShootAudio");
        }

    }

    public void StartIntroFlight(Vector3 targetPosition)
    {
        startTarget = targetPosition;
        transform.position = new Vector3(0f, -Camera.main.orthographicSize - 10f, 0f); // ngoài màn hình dưới
        isStarting = true;

    }

    private IEnumerator StartMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

    }


    // Kiểm tra va chạm với các đối tượng khác
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
        other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            TakeDamage();
        }
    }

    // Hàm xử lý khi nhân vật bị trúng đạn
    public void TakeDamage()
    {
        if (isInvincible || lives <= 0) return;

        lives--;
        // Cập nhật giao diện hiển thị mạng sống của nhân vật
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        // Kiểm tra nếu mạng sống <= 0
        if (lives <= 0)
        {
            // Game Over
            uiManager.ShowGameOver();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("GameOver");
            }


        }
        else
        {
            // Bật chế độ bất tử
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    // Coroutine để xử lý chế độ bất tử
    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool visible = true;

        // Bắt đầu nhấp nháy    
        while (elapsed < invincibilityDuration)
        {
            visible = !visible;
            spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }

        spriteRenderer.enabled = true; // bật lại nếu đang tắt
        isInvincible = false;

    }

}
