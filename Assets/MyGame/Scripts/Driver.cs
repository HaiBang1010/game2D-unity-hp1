using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// dùng để điều khiển nhân vật máy bay của bản thân
public class Driver : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    public Projectile laserPrefab;
    // Số mạng của người chơi
    public int lives = 3;
    public Image[] hearts;
    //làm nhấp nháy khi bị trúng đạn
    private bool isInvincible = false;
    public float invincibilityDuration = 1.5f; // thời gian bất tử sau khi trúng đòn
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //di chuyển động của player
        float changeSteerH = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float changeSteerV = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(changeSteerH, changeSteerV, 0);

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
            Debug.Log("Game Over!"); // hoặc load scene Game Over
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
