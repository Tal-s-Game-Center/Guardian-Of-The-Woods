using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private Mover mover;       // Reference to the Mover script

    [Range(0.1f, 3f)] // Slider for animation speed in the Inspector
    public float animationSpeed = 1f;

    private void Start()
    {
        // Get references to the Animator and Mover components
        animator = GetComponent<Animator>();
        mover = GetComponent<Mover>();

        // Set the initial animation speed
        animator.speed = animationSpeed;
    }

    private void Update()
    {
        // Update the Animator speed dynamically
        animator.speed = animationSpeed;

        // Get the magnitude of the movement speed from the Mover script
        if (mover.isMoving)
        {
            animator.SetBool("isWalking", true); // Trigger walking animation
        }
        else
        {
            animator.SetBool("isWalking", false); // Trigger idle animation
        }
    }
}
