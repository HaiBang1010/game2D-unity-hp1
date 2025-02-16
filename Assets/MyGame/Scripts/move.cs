using UnityEngine;

public class move : MonoBehaviour
{
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");  // Nhận input từ A/D hoặc phím mũi tên
        float moveY = Input.GetAxis("Vertical");  // Nhận input từ S/W hoặc phím mũi tên
        transform.position += new Vector3(moveX, moveY, 0)*moveSpeed * Time.deltaTime;
        
    }
}
