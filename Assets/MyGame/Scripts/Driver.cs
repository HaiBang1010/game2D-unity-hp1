using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    public Projectile laserPrefab;

    private bool laserActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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


}
