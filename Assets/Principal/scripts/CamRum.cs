using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRum : MonoBehaviour
{
    Rigidbody rb;

    public float Velocity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * Velocity;
    }
}
