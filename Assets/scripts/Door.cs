using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool opened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            // Assign this door to the player
            other.gameObject.GetComponent<PlayerController>().UpdateDoor(this);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            // Remove this door from the player
            other.gameObject.GetComponent<PlayerController>().UpdateDoor(null);
        }
    }
    /// <summary>
    /// door movement snap movement, no animation
    /// </summary>
    public void OpenDoor()
    {
        if (!opened)
        {
            Vector3 newRotation = transform.eulerAngles;
            newRotation.y += 90f;
            transform.eulerAngles = newRotation;
            opened = true;
        }
    }
}