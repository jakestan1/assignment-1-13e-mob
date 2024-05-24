using System.Collections;
using UnityEngine;

public class ZoneRegen : MonoBehaviour
{
    [SerializeField] private GameObject regenMessage;
    [SerializeField] private Player playerScript;

    private Coroutine regenCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            regenMessage.SetActive(true); // Show the regeneration message
            if (regenCoroutine == null)
            {
                regenCoroutine = StartCoroutine(RegenPlayer());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            regenMessage.SetActive(false); // Hide the regeneration message
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
    }

    private IEnumerator RegenPlayer()
    {
        while (true)
        {
            playerScript.slider1.value += 0.1f;
            yield return new WaitForSeconds(5);
        }
    }
}
