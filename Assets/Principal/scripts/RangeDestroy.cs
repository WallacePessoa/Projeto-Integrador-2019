using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coletavel")|| other.CompareTag("Parede")|| other.CompareTag("Enemi"))
        {
            Destroy(other.gameObject);
        }
    }
}
