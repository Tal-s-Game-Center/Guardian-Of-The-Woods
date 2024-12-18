using UnityEngine;

public class DoorActivator : MonoBehaviour
{
    // Reference to the door object in the Inspector
    public GameObject door; // Assign the door object in the Inspector

    // Reference to the DoorRotation script on the door object
    private DoorRotation doorRotationScript;

    // Start is called before the first frame update
    void Start()
    {
        if (door != null)
        {
            // Get the DoorRotation script from the door object
            doorRotationScript = door.GetComponent<DoorRotation>();

            if (doorRotationScript == null)
            {
                Debug.LogError("DoorRotation script not found on the door object!");
            }else{

            }
        }
        else
        {
            Debug.LogError("Door object is not assigned in the Inspector!");
        }
    }

    // Method to activate the door rotation
    public void ActivateRotation()
    {
        if (doorRotationScript != null)
        {
            // Enable the rotation by starting the coroutine in the DoorRotation script
            doorRotationScript.StartRotation();
        }
    }
}
