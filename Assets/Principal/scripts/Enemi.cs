using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemi : MonoBehaviour
{
    public Color corMira;
    Material Material;

    public GameObject Mira;
    public GameObject Player;


    bool AtirarPlayer = false;
   
    void Start()
    {
        //corMira = Mira.GetComponent<Color>();
        //Material = Mira.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // if (Vector3.Distance(Player.transform.position, transform.position) < 15 && Vector3.Distance(Player.transform.position, transform.position) > 5)
       // {

       //     //Material.SetColor("Mira", Color.green);
       //     Material.color = corMira;

       // }
       //else
       //     Material.color = corMira;


        //print(Vector3.Distance(Player.transform.position, transform.position));


        if (AtirarPlayer)
        {
            if (Input.GetKey(KeyCode.X))
            {
                Destroy(gameObject);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(Mira, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Player.transform.rotation, transform);
        if (other.CompareTag("Player"))
        {
            AtirarPlayer = true;
        }

    }
}
