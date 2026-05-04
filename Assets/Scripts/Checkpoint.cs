using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.lastCheckpoint = this.transform;
            }
        }
    }
}
