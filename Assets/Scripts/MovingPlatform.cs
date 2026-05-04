using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float distance = 5f;
    public bool horizontal = true;

    private Vector2 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (horizontal)
        {
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
            {
                direction *= -1;
            }
        }
        else
        {
            transform.Translate(Vector2.up * direction * moveSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.position.y - startPos.y) >= distance)
            {
                direction *= -1;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Parenting the player so they move with the platform
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            // Required fix so player scale doesn't mess up if platform is scaled
            Vector3 scale = collision.transform.localScale;
            collision.transform.localScale = new Vector3(Mathf.Abs(scale.x) * Mathf.Sign(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
        }
    }
}
