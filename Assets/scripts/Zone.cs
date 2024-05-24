using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public GameObject zoneMessage;

    public Enemy enemyScript;


    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Wait());
        }
    }



    private IEnumerator Wait()
    {
        zoneMessage.SetActive(true);
        enemyScript.enabled = true;

        yield return new WaitForSeconds(5);

        zoneMessage.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
