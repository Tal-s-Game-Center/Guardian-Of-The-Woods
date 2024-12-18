using UnityEngine;

public class Seed : MonoBehaviour
{
    private bool isPickedUp = false; // Tracks if the seed is picked up
    private Transform parentTransform; // Reference to the parent transform
    private Collider seedCollider;
    private Rigidbody seedRigidbody;

    [SerializeField] private string placeholderTag = "SeedHolder"; // Tag to identify seed holders
    [SerializeField] private float snapRadius = 5f; // Radius to check for seed holders

    private void Start()
    {
        seedCollider = GetComponent<Collider>();
        seedRigidbody = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform newParent)
    {
        // Set the parent transform
        parentTransform = newParent;
        transform.SetParent(newParent);

        // Disable physics interaction while picked up
        if (seedRigidbody != null)
        {
            seedRigidbody.isKinematic = true;
        }

        // Disable collisions while picked up
        if (seedCollider != null)
        {
            seedCollider.isTrigger = true;
        }

        isPickedUp = true; // Mark as picked up
        Debug.Log("Seed picked up!");
    }

    public void Drop()
    {
        // Remove the parent transform
        isPickedUp = false;
        parentTransform = null;
        transform.SetParent(null);

        // Enable physics interaction again
        if (seedRigidbody != null)
        {
            seedRigidbody.isKinematic = false;
        }

        // Enable collisions
        if (seedCollider != null)
        {
            seedCollider.isTrigger = false;
        }

        // Check for nearby placeholders and snap if within range
        TrySnapToPlaceholder();

        Debug.Log("Seed dropped!");
    }

    private void TrySnapToPlaceholder()
    {
        GameObject[] placeholders = GameObject.FindGameObjectsWithTag(placeholderTag);
        SeedHolder closestHolder = null;
        float closestDistance = snapRadius;

        foreach (GameObject placeholder in placeholders)
        {
            // Check if the placeholder is within the snap radius
            float distance = Vector3.Distance(transform.position, placeholder.transform.position);
            if (distance <= snapRadius)
            {
                if (closestHolder == null || distance < closestDistance)
                {
                    closestHolder = placeholder.GetComponent<SeedHolder>();
                    closestDistance = distance;
                }
            }
        }

        // If a valid holder is found, snap the seed to it
        if (closestHolder != null)
        {
            closestHolder.TrySnapSeed(gameObject);
        }
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }
}
