using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Driver : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    public Projectile laserPrefab;

    public int lives;
    public Image[] hearts;
    private bool laserActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lives = hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //di chuyển động của player
        float changeSteerH = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float changeSteerV = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(changeSteerH, changeSteerV, 0);
        // transform.Rotate(0, -changeSteerV * 2, 0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, this.laserPrefab.transform.rotation);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
        other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        lives--;

        hearts[lives].gameObject.SetActive(false);

        if (lives <= 0)
        {
            // Game Over
            Debug.Log("Game Over!"); // hoặc load scene Game Over
        }
    }

}
