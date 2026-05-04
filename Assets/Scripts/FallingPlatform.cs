using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f;
    public float respawnDelay = 3f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Start static
        startPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(FallAndRespawn());
        }
    }

    private IEnumerator FallAndRespawn()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 4f;
        
        yield return new WaitForSeconds(respawnDelay);
        
        // Reset
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.position = startPos;
        isFalling = false;
    }
}
