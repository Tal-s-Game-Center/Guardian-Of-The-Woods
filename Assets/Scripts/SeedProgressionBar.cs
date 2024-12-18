using UnityEngine;
using System;
using Unity.VisualScripting;

public class SeedProgressionBar : MonoBehaviour
{
    [Header("Progression Bar Settings")]
    public float fillingRate = 0.1f;  // Rate at which the bar fills (adjustable)
    private float currentProgress = 0f;  // Current progress of the bar (0 to 1)

    [SerializeField] private float rotationSpeed = 50f; // Rotation speed before full progress
    [SerializeField] private float maxRotationSpeed = 500f; // Max rotation speed after progression is complete
    [SerializeField] private float shrinkSpeed = 1f; // Speed at which the cube shrinks after finishing
    [SerializeField] private float shrinkDuration = 1f; // Duration for the cube to shrink
    [SerializeField] private float rotationSpeedIncreaseRate = 1.01f; // Speed of rotation speed increase (exponential growth)

    private Renderer cubeRenderer;
    private Vector3 originalScale;

    private Color startColor = Color.red;  // Color when progress is 0
    private Color endColor = Color.green;  // Color when progress is 1
    private Color activeColor = new Color(0.5f, 0.8f, 1f); // Light blue color when active

    private bool isFilling = false;  // Whether the bar is actively filling
    private bool isRotating = true;  // Whether the cube is rotating
    private bool isShrinking = false;  // Whether the cube is shrinking after full progress

    private void Start()
    {
        // Store the original scale of the cube
        originalScale = transform.localScale;

        // Get the cube's renderer to change color
        cubeRenderer = GetComponent<Renderer>();

        // Set the cube to be invisible initially
        cubeRenderer.material.color = startColor; // Start with the "starting" color (red)
        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);  // Set the initial width to 0
    }

    private void Update()
    {
        // Rotate the cube around the Y-axis when not filling and not shrinking
        if (isRotating && !isShrinking)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate at the current speed
        }

        // If the progression bar is active and filling, update the progress
        if (isFilling)
        {
            // Stop rotating and set the cube to 0 degrees rotation when the progression starts
            isRotating = false;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Change the color to light blue when the bar starts filling
            cubeRenderer.material.color = activeColor;

            // Increase the progress over time
            currentProgress += fillingRate * Time.deltaTime;

            // Make sure the progress does not exceed 1 (full)
            if (currentProgress >= 1f)
            {
                currentProgress = 1f;
                isFilling = false;  // Stop filling when it's full

                // Trigger success behavior
                StartCoroutine(HandleSuccess());
            }

            // Update the scale of the cube (only on the X axis)
            float newXScale = Mathf.Lerp(0f, 0.03f, currentProgress); // Scale from 0 to 0.03 based on progress
            transform.localScale = new Vector3(newXScale, originalScale.y, originalScale.z);

            // Change the color based on progress
            cubeRenderer.material.color = Color.Lerp(startColor, endColor, currentProgress);
        }

        // If shrinking, decrease the scale over time
        if (isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

            // When the scale is small enough, hide the object
            if (transform.localScale.x < 0.01f)
            {
                gameObject.SetActive(false); // Hide the cube once it's sufficiently shrunk
            }
        }
    }

    // Call this method to start the progression
    public void StartProgression()
    {
        isFilling = true;
        rotationSpeed = 2;
    }

    // Call this method to stop or reset the progression
    public void ResetProgression()
    {
        currentProgress = 0f;
        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        cubeRenderer.material.color = startColor;  // Reset to red
        isFilling = false;
        isRotating = true;  // Start rotating again
        isShrinking = false;  // Reset shrinking state
    }

    // Handle the success state after the bar is full
    private System.Collections.IEnumerator HandleSuccess()
    {
        // Gradually increase rotation speed exponentially
        float initialSpeed = rotationSpeed;
        while (rotationSpeed < maxRotationSpeed)
        {
            rotationSpeed *= rotationSpeedIncreaseRate;
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Start shrinking after the rotation speed limit is reached
        isShrinking = true;
        SeedHolder seedHolder = gameObject.GetComponentInParent<SeedHolder>() ;
        if(seedHolder == null){ 
            Debug.Log("Shit!") ;            
        }
        seedHolder.TriggerExplosion() ;
        rotationSpeed = initialSpeed;
    }
}
