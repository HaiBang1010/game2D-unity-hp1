using UnityEngine;

public class Invader : MonoBehaviour
{
    // public Sprite[] animationSprites;
    // public float animationTime = 1.0f;
    // private SpriteRenderer spriteRenderer;
    // private int animationFrame;
    public System.Action killed;
    // private void Awake()
    // {
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    // }

    // private void Start()
    // {
    //     InvokeRepeating(nameof(AnimateSprite), this.animationTime,  this.animationTime);
    // }

    // private void AnimateSprite()
    // {
    //     animationFrame ++;
    //     if (animationFrame >= animationSprites.Length)
    //     {
    //         animationFrame = 0;
    //     }
    //     spriteRenderer.sprite = animationSprites[animationFrame];
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Debug.Log("Invader hit by projectile!");
            this.killed?.Invoke();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
