using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBallSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject auraBallPrefab; // Prefab for the aura balls
    [SerializeField] private float initialSpawnRadius = 7f; // Initial radius for spawning
    [SerializeField] private float initialMoveSpeed = 3f; // Initial speed of balls moving towards the holder
    [SerializeField] private float radiusDecreaseRate = 0.1f; // How much the radius increases over time
    [SerializeField] private float speedIncreaseRate = 0.05f; // How much the speed increases over time

    [Header("Aura Ball Settings")]
    [SerializeField] private Color[] auraColors; // Array of colors for the aura balls
    [SerializeField] private Vector2 ballSizeRange = new Vector2(0.2f, 0.5f); // Random size range for balls

    private List<GameObject> activeAuraBalls = new List<GameObject>();
    private bool isExploding = false; // Track if the explosion has started

    public void StartSpawning()
    {
        Debug.Log("StartSpawning");
        StartCoroutine(SpawnAuraBalls());
    }

    private IEnumerator SpawnAuraBalls()
    {
        float spawnRadius = initialSpawnRadius;
        float moveSpeed = initialMoveSpeed;

        while (!isExploding)
        {
            // Spawn a new ball
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y += Random.Range(1f, 3f); // Spawn balls above ground height

            // Instantiate the aura ball
            GameObject ball = Instantiate(auraBallPrefab, randomPos, Quaternion.identity);
            float randomSize = Random.Range(ballSizeRange.x, ballSizeRange.y);
            ball.transform.localScale = Vector3.one * randomSize;

            // Assign a random color
            Renderer ballRenderer = ball.GetComponent<Renderer>();
            if (ballRenderer != null && auraColors.Length > 0)
            {
                ballRenderer.material.color = auraColors[Random.Range(0, auraColors.Length)];
            }

            activeAuraBalls.Add(ball); // Keep track of active balls
            StartCoroutine(MoveAndRotateBall(ball, spawnRadius));

            // Wait before spawning the next ball
            yield return new WaitForSeconds(0.1f);

            // Gradually increase the spawn radius and speed
            spawnRadius -= radiusDecreaseRate;
            moveSpeed += speedIncreaseRate;
        }
    }

    private IEnumerator MoveAndRotateBall(GameObject ball, float spawnRadius)
    {
        // Random rotation axis
        Vector3 rotationAxis = Random.onUnitSphere * 360f;

        while (!isExploding && ball != null)
        {
            // Move the ball towards the center (holder)
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, transform.position, initialMoveSpeed * Time.deltaTime);

            // Apply continuous random spin
            ball.transform.Rotate(rotationAxis * Time.deltaTime);

            // Shrink the ball as it approaches the center
            float distanceToCenter = Vector3.Distance(ball.transform.position, transform.position);
            float shrinkFactor = Mathf.Clamp01(distanceToCenter / spawnRadius); // Scale down as it moves closer
            ball.transform.localScale = Vector3.one * shrinkFactor * 0.5f;

            // Destroy the ball when it reaches the center
            if (distanceToCenter < 0.2f)
            {
                Destroy(ball);
                activeAuraBalls.Remove(ball);
                yield break;
            }

            yield return null;
        }
    }

    public void ExplodeBalls()
    {
        if (isExploding) return;

        isExploding = true;

        foreach (GameObject ball in activeAuraBalls)
        {
            if (ball == null) continue;

            // Apply explosive force in a random outward direction
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = ball.AddComponent<Rigidbody>(); // Add Rigidbody if it doesn't exist
            }

            rb.isKinematic = false;
            Vector3 explosionDir = (ball.transform.position - transform.position).normalized + Random.insideUnitSphere;
            rb.AddForce(explosionDir * Random.Range(5f, 15f), ForceMode.Impulse);

            // Destroy the ball after a short delay
            Destroy(ball, 2f);
        }

        activeAuraBalls.Clear(); // Clear the list
    }
}
