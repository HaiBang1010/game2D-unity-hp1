using System;
using UnityEngine;

public class ColliSquare : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("Collision detected with: " + collision.gameObject.name);

        spriteRenderer.color = new Color(0, 154, 255);
    }

}