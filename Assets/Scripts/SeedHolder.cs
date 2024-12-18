using UnityEngine;

public class SeedHolder : MonoBehaviour
{
    [SerializeField] private Vector3 snapOffset = Vector3.zero; // Offset for snapped seeds
    [SerializeField] private float detectionRadius = 5f; // Radius to detect seeds
    [SerializeField] private string seedTag = "Seed"; // Tag to identify seed objects
    [SerializeField] private SeedProgressionBar progressionBar; // Reference to the SeedProgressionBar component
    [SerializeField] private AuraBallSpawner auraBallSpawner; // Reference to AuraBallSpawner
    [SerializeField] private DoorActivator doorActivator ;
    private bool hasExploded = false; // Prevents multiple explosions

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public bool TrySnapSeed(GameObject seed)
    {
        // Check if the seed is within the detection radius
        if (Vector3.Distance(transform.position, seed.transform.position) <= detectionRadius)
        {
            SnapSeed(seed);
            return true;
        }

        return false;
    }

    private void SnapSeed(GameObject seed)
    {
        // Set the seed's parent to the holder
        seed.transform.SetParent(transform);

        // Set the seed's local position to the offset
        seed.transform.localPosition = snapOffset;

        // Reset the seed's rotation
        seed.transform.localRotation = Quaternion.identity;

        // Disable the seed's physics
        Rigidbody seedRigidbody = seed.GetComponent<Rigidbody>();
        if (seedRigidbody != null)
        {
            seedRigidbody.isKinematic = true;
        }

        // Activate the progression bar when the seed is snapped
        if (progressionBar != null)
        {
            progressionBar.StartProgression(); // Start the progression
        }

        // Start spawning aura balls
        if (auraBallSpawner != null)
        {
            auraBallSpawner.StartSpawning();
        }

        Debug.Log($"Seed snapped to holder {gameObject.name} at local offset {snapOffset}!");
    }

    public void TriggerExplosion()
    {
        if (!hasExploded && auraBallSpawner != null)
        {
            hasExploded = true;

            // Trigger aura balls explosion
            auraBallSpawner.ExplodeBalls();
            doorActivator.ActivateRotation();
            Debug.Log("Progress bar complete! Aura balls exploded.");
        }
    }
}
