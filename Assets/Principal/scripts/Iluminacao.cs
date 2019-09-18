using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iluminacao : MonoBehaviour
{


    public GameObject Player;
    public GameObject Predio;
    public List<Light> Lampadas = new List<Light>();
    public List<GameObject> Objetos = new List<GameObject>();
    public bool atv = false;

    Material Mt;
    Color Cor;

    int rnd;



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
            //print(tranparente);

        }

        yield return null;
        StartCoroutine(Fade());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rnd = Random.Range(1, 15);
            if (rnd < 14)
            {
                for (int x = 0; x < Objetos.Count; x++)
                {
                    Objetos[x].gameObject.SetActive(false);
                }
            }
            else {

                for (int x = 0; x < Objetos.Count; x++)
                {
                    Objetos[x].gameObject.SetActive(true);
                }


            }

            if (atv)
            {
                print(rnd);

            }

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 375f);
        }
    }
}
