using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    private Vector3 offset; // Offset from the player

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not set and no parent transform exists!");
            return;
        }

        // Calculate the initial offset between the camera and the player
        offset = transform.position - player.position;

        offset.x = 0f ;
    }

    void LateUpdate()
    {
        // If player is null (e.g., destroyed at runtime), stop further execution
        if (player == null) return;

        // Maintain the camera's position relative to the player
        transform.position = player.position + offset;
    }
}
