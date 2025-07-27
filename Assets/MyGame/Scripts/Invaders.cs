using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs = new Invader[4];

    public GameObject bossHealthUI;
    public GameObject bossPrefab;
    private bool bossSpawned = false;

    public Projectile missilePrefab;
    public int rows = 4, columns = 10; // 4 hàng, mỗi hàng 10 con quái vật

    public int totalInvaders => this.rows * this.columns; // Tổng số quái vật

    public int amountLived => this.totalInvaders - this.amountKilled; // Số quái vật còn sống

    private float speed = 1.5f; // Tốc độ di chuyển của quái vật

    private Vector3 direction = Vector2.right; // Hướng di chuyển của quái vật, ban đầu là sang phải

    public int amountKilled { get; private set; } = 0; // Số quái vật đã bị tiêu diệt

    public float missileSpawnRate = 1.0f;// Tần suất bắn tên lửa của quái vật

    public void Start() { }


    public void BeginInvaders()
    {
        SpawnInvaders();
        StartCoroutine(StartMissileAttackAfterDelay(2f)); // delay 2 giây
    }

    private IEnumerator StartMissileAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    public void Update()
    {
        // Di chuyển quái vật
        this.transform.position += this.direction * this.speed * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Kiểm tra xem quái vật có chạm vào mép trái hoặc mép phải của màn hình
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f))
            {
                AdvanceRow();
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }

        }

    }

    // Hàm để sinh ra quái vật
    public void SpawnInvaders()
    {
        for (int row = 0; row < this.rows; row++)
        {
            float height = this.columns - 1;//36
            float width = 4.0f * (this.rows - 1);//16

            // Tính toán vị trí của hàng quái vật
            Vector2 center = new Vector2(-width, height / 3.5f);//-18, 8
            Vector3 rowPosition = new Vector3(center.x, row * 2.0f + center.y, 0.0f);
            // Tạo ra các quái vật trong hàng   
            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);

                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;

                position.x += col * 3.0f;
                position.y += row * 1.5f;
                Vector3 startPos = position + new Vector3((col % 2 == 0 ? -10f : 10f), 0f, 0f);
                invader.transform.position = startPos;
                StartCoroutine(MoveInvaderToPosition(invader.transform, position));
            }
        }
    }

    private IEnumerator MoveInvaderToPosition(Transform invader, Vector3 targetPos)
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 start = invader.position;

        while (elapsed < duration)
        {
            invader.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;

        }

        invader.position = targetPos;
    }

    private void AdvanceRow()
    {
        direction.x *= -1;
        // Dịch chuyển toàn bộ quái vật xuống dưới 1 khoảng
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        // Bước 1: gom tất cả invader còn sống vào 1 danh sách
        List<Transform> aliveInvaders = new List<Transform>();

        foreach (Transform invader in this.transform)
        {
            if (invader.gameObject.activeInHierarchy)
            {
                aliveInvaders.Add(invader);
            }
        }

        // Bước 2: nếu còn invader sống thì chọn random
        if (aliveInvaders.Count > 0)
        {
            int randomIndex = Random.Range(0, aliveInvaders.Count);
            Transform randomInvader = aliveInvaders[randomIndex];

            Instantiate(this.missilePrefab, randomInvader.position, this.missilePrefab.transform.rotation);
        }

    }
    private void InvaderKilled()
    {
        // Khi một invader bị tiêu diệt, tăng số lượng đã tiêu diệt
        this.amountKilled++;
        if (this.amountKilled >= this.totalInvaders)
        {
            // GameManager.Instance.OnLevelComplete();
            bossSpawned = true;
            StartCoroutine(SpawnBossDelayed());
        }
    }

    private IEnumerator SpawnBossDelayed()
    {
        yield return new WaitForSeconds(3f); // đợi 3 giây
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        // Vector3 spawnPos = new Vector3(0f, Camera.main.orthographicSize - 1.5f, 0f);
        Vector3 spawnPos = new Vector3(0f, 8f, 0f);
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        Boss bossScript = bossObj.GetComponent<Boss>();
        bossScript.InitBossUI(bossHealthUI);
    }
}
