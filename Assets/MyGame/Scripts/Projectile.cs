using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float speed = 25f;
    public System.Action onDestroy;


    void Update()
    {
        this.transform.position += this.transform.right * this.speed * Time.deltaTime;

        // a += 3; a = a +3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag("Enemy"))
        this.onDestroy?.Invoke();
        Destroy(this.gameObject);
    }
}
