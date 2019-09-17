using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacao : MonoBehaviour
{


    public GameObject Player;
    public GameObject Predio;
    public List<Light> Lampadas = new List<Light>();
    public bool atv = false;

    Material Mt;
    Color Cor;



    float tranparente;

    void Start()
    {
        Mt = Predio.GetComponent<Renderer>().material;


        tranparente = 0;

        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public IEnumerator Fade()
    {

        tranparente = 0;




        for (float x = 0; x <= 374; x += 3.74f)
        {


            if (Vector3.Distance(transform.position, Player.transform.position) <= x)
            {
                tranparente += 0.01f;

                Cor.a = tranparente;
                Mt.color = Cor;
                Predio.GetComponent<Renderer>().material = Mt;

                //for(int y = 0; y < Lampadas.Count; y++)
                //{
                //    Lampadas[y].intensity = tranparente;
                //}



            }


        }


        if (atv)
        {
            print(tranparente);

        }

        yield return null;
        StartCoroutine(Fade());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 375f);
        }
    }
}
