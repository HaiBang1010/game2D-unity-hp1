using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private Slider healthSlider;
    private GameObject healthUI;
    public float moveSpeed = 3f;
    private Vector3 direction = Vector3.right;

    public Projectile bossProjectile;
    public float shootRate = 2f; // thời gian giữa các lần bắn

    public Transform[] firePoints; // điểm bắn nhiều hướng
    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        Move();
    }


    public void InitBossUI(GameObject bossHealthObject)
    {
        this.healthUI = bossHealthObject;
        healthUI.SetActive(true);

        healthSlider = healthUI.GetComponent<Slider>();
        if (healthSlider != null)
        {

            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
        else
        {
            Debug.LogError("Health Slider not found in Boss Health UI");
        }
    }

    void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Giới hạn trái phải màn hình
        float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        if (transform.position.x >= cameraWidth || transform.position.x <= -cameraWidth)
        {
            direction *= -1;
        }
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootRate);

            // Bắn đạn tỏa ra theo các góc
            float[] angles = new float[] { -60f, -40f, -20f, 0f, 20f, 40f, 60f };
            foreach (float angle in angles)
            {

                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);

                Instantiate(this.bossProjectile, transform.position, rotation);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthSlider != null)
            healthSlider.value = currentHealth;
        Debug.Log("Boss Health: " + healthSlider.value);

        if (currentHealth <= 0)
        {
            Die();
            UIManager.Instance.ShowWinMenu();
        }
    }

    void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Game Over Win, Load scene, etc.
        Destroy(this.gameObject);

        healthUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
