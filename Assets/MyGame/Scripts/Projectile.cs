using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 25f;
    public System.Action onDestroy;



    void Update()
    {
        // Di chuyển tên lửa theo hướng đã chỉ định
        this.transform.position += this.transform.right * this.speed * Time.deltaTime;
    }

    // Hàm để xử lý va chạm với các đối tượng khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Enemy"))
        this.onDestroy?.Invoke();
        Destroy(this.gameObject);
    }
}
