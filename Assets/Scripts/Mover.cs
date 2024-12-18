using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed = 5f;   // Movement speed
    public bool isMoving = false; // Indicates if the character is currently moving

    [SerializeField]
    public InputAction moveAction;

    private Vector3 currentVelocity = Vector3.zero; // For smooth stopping
    private Vector3 moveDirection = Vector3.zero;   // Cached movement direction

    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody settings for smooth movement
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotation
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth physics-based movement
    }

    public void OnEnable()
    {
        moveAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
    }

    private void FixedUpdate()
    {
        // Read movement input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveDirection != Vector3.zero)
        {
            isMoving = true;

            // Calculate new position and move the character
            Vector3 targetPosition = transform.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Smoothly rotate the character to face the movement direction
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
        else
        {
            isMoving = false;
        }
    }
}
