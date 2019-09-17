using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Running : MonoBehaviour
{

    Rigidbody rb;
    public GameObject TargetFinish;
    public float Velocity = 5;

    RaycastHit hit;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, 30);
        Debug.DrawRay(transform.position, transform.forward * 30);

        transform.LookAt(TargetFinish.transform.position);

   
        //rb.AddForce(transform.forward * Velocity);
        rb.velocity = transform.forward * Velocity;


        
    }
}
