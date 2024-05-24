using System.Collections;
using UnityEngine;

public class CoinZone : MonoBehaviour
{
    [SerializeField] private GameObject regenMessage;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if there are any collectable objects within the CoinZone
            if (HasCollectables())
            {
                regenMessage.SetActive(true); // Show the regeneration message
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            regenMessage.SetActive(false); // Hide the regeneration message
        }
    }

    /// <summary>
    /// Checks if there are any collectables within the CoinZone.
    /// </summary>
    /// <returns>True if there are collectables, otherwise false.</returns>
    private bool HasCollectables()
    {
        // Iterate through all child objects to check for collectables
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Collectables"))
            {
                return true;
            }
        }
        return false;
    }
}
