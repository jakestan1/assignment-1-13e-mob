/*
 * Author: 
 * Date: 
 * Description: Handles coin collection mechanics and shows/hides a congratulatory message for a specified duration.
 */

using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject congratsMessage; // UI message to show when the coin is collected
    [SerializeField] private Player playerScript; // Reference to the player script (not used in this code)
    [SerializeField] private float displayDuration = 2f; // Duration to display the congratulatory message

    /// <summary>
    /// Stores the score that each coin is worth.
    /// </summary>
    public int coinScore;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayCongratsMessage()); // Start the coroutine to display the message
            Collected(); // Collect the coin
        }
    }

    void OnTriggerExit(Collider other)
    {
        // No need to hide the message here
    }

    /// <summary>
    /// The function to use when the coin is collected.
    /// </summary>
    public void Collected()
    {
        Destroy(gameObject); // Destroy the coin game object
    }

    /// <summary>
    /// Coroutine to display the congratulatory message for a specific duration.
    /// </summary>
    private IEnumerator DisplayCongratsMessage()
    {
        congratsMessage.SetActive(true); // Show the congrats message
        yield return new WaitForSeconds(displayDuration); // Wait for the specified duration
        congratsMessage.SetActive(false); // Hide the congrats message
    }
}
