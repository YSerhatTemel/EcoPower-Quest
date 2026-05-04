using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(4f, 1f, -10f); // Look slightly ahead and up
    public float minY = 0f; // Minimum Y height so it doesn't show the void below

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        
        // Prevent the camera from dipping too far below the platforms
        if (desiredPosition.y < minY)
        {
            desiredPosition.y = minY;
        }
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
