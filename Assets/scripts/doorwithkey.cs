using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorwithkey : MonoBehaviour
{
    bool opened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.Key)
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        // Get the current rotation angles
        Vector3 newRotation = transform.eulerAngles;
        // Add 90 degrees to the y-axis rotation
        newRotation.y += 90f;
        // Apply the new rotation to the transform
        transform.eulerAngles = newRotation;
        opened = true;
    }
}




