using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    // The point around which the door will rotate, relative to its parent.
    public Vector3 rotationPoint = new Vector3(-0.5f, 0, 0); // Example: hinge at the left edge of the door
    public float rotationSpeed = 30f; // Rotation speed in degrees per second
    public float targetRotation = 105f; // Target rotation in degrees

    private float currentRotation = 0f; // Track the current rotation of the door
    private bool isRotating = false; // Flag to check if the door is currently rotating

    // Update is called once per frame
    void Update()
    {
        // Check if the door is rotating and has not reached the target rotation
        if (isRotating && currentRotation < targetRotation)
        {
            // Convert the local rotation point to world space (hinge position)
            Vector3 worldRotationPoint = transform.parent.TransformPoint(rotationPoint);

            // Rotate the door around the hinge (Y-axis)
            transform.RotateAround(worldRotationPoint, Vector3.up, rotationSpeed * Time.deltaTime);

            // Increment the current rotation based on the rotation speed
            currentRotation += rotationSpeed * Time.deltaTime;

            // Ensure the rotation does not exceed the target rotation
            if (currentRotation >= targetRotation)
            {
                currentRotation = targetRotation;
                isRotating = false; // Stop rotation once the target is reached
            }
        }
    }

    // Method to start the door rotation
    public void StartRotation()
    {
        if (!isRotating)
        {
            Debug.Log("Start rotating") ;
            isRotating = true; // Start rotation
            currentRotation = 0f; // Reset current rotation to 0
        }
    }

    // Optional: Draw the Gizmo to visualize the rotation point
    void OnDrawGizmos()
    {
        // Get the world position of the rotation point
        Vector3 worldRotationPoint = transform.parent.TransformPoint(rotationPoint);

        // Draw a sphere at the rotation point in the scene view
        Gizmos.color = Color.red; // Set the color for the Gizmo (red for visibility)
        Gizmos.DrawSphere(worldRotationPoint, 0.1f); // Draw a sphere with a radius of 0.1
    }
}
