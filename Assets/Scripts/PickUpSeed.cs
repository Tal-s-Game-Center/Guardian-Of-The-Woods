using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpSeed : MonoBehaviour
{
    [SerializeField] private InputAction pickUpAction; // InputAction to trigger the pickup
    [SerializeField] private float detectionRadius = 5f; // Radius to search for objects with the "Seed" tag
    [SerializeField] private Vector3 seedOffset = new Vector3(0, 0.5f, 0); // Offset to position the seed relative to the player
    [SerializeField] private Quaternion seedRotation = Quaternion.identity; // Optional rotation offset for the seed

    private Seed pickedUpSeed; // Reference to the currently picked-up seed

    private void OnEnable()
    {
        pickUpAction.Enable();
    }

    private void OnDisable()
    {
        pickUpAction.Disable();
    }

    private void Update()
    {
        // Check if the pick-up action was performed
        if (pickUpAction.WasPerformedThisFrame())
        {
            // If the player already has a seed, drop it
            if (pickedUpSeed != null)
            {
                DropSeed();
                return;
            }

            // Find all colliders within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (Collider collider in colliders)
            {
                // Check if the collider has the "Seed" tag
                if (collider.CompareTag("Seed"))
                {
                    // Get the Seed script component
                    Seed seed = collider.GetComponent<Seed>();
                    if (seed != null && !seed.IsPickedUp())
                    {
                        // Pick up the seed
                        PickUpSeedObject(seed);
                        Debug.Log("Seed picked up and moved to the player!");
                        break; // Exit after handling the first seed
                    }
                }
            }
        }

        // Update the seed's position relative to the player if picked up
        if (pickedUpSeed != null)
        {
            // Position the seed relative to the player
            pickedUpSeed.transform.localPosition = seedOffset;
            pickedUpSeed.transform.localRotation = seedRotation;
        }
    }

private void PickUpSeedObject(Seed seed)
{
    // Set the seed's parent to the player
    seed.transform.SetParent(transform);

    // Ensure the seed's scale is maintained
    seed.transform.localScale = new Vector3(30f, 30f, 30f) ; // Reset scale to avoid stretching

    // Apply the position and rotation offsets relative to the player
    seed.transform.localPosition = seedOffset;
    seed.transform.localRotation = seedRotation;

    // Disable physics interaction on the seed
    Rigidbody seedRigidbody = seed.GetComponent<Rigidbody>();
    if (seedRigidbody != null)
    {
        seedRigidbody.isKinematic = true;
    }

    // Mark the seed as picked up
    seed.PickUp(transform);
    pickedUpSeed = seed; // Store the reference to the picked-up seed
}


    private void DropSeed()
    {
        if (pickedUpSeed != null)
        {
            // Drop the seed
            pickedUpSeed.Drop();

            // Enable physics interaction for the dropped seed
            Rigidbody seedRigidbody = pickedUpSeed.GetComponent<Rigidbody>();
            if (seedRigidbody != null)
            {
                seedRigidbody.isKinematic = false;
            }

            pickedUpSeed = null; // Clear the reference
            Debug.Log("Seed dropped!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
