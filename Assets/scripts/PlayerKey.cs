using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKey : MonoBehaviour
{
    public bool Key = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("key"))
        {
            Destroy(other.gameObject);
            Key = true;
        }

        // Existing code for other tags...
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
