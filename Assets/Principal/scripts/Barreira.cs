using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour
{
    Color cor;
    public float CorR;
    public float Corg;
    public float Corb;

    void Start()
    {
        StartCoroutine(Chamar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator Chamar()
    {
        while(cor.r<=1|| cor.b <= 1|| cor.g <= 1)
        {
            for (float x = 0; x < 1; x += 0.01f)
            {
                cor = GetComponent<Renderer>().material.color;
                if (cor.b < 1)
                    cor.b += x;
                else if (cor.r < 1)
                    cor.r += x;
                else if (cor.g < 1)
                    cor.g += x;

                GetComponent<Renderer>().material.color = cor;

                yield return new WaitForSeconds(0.1f);
            }
        }

        while (cor.r >= 0|| cor.b >= 0 || cor.g >= 0)
        {
            for (float x = 1; x > 0; x -= 0.01f)
            {
                cor = GetComponent<Renderer>().material.color;
                if (cor.b > 0)
                    cor.b -= x;
                else if (cor.r > 0)
                    cor.r -= x;
                else if (cor.g > 0)
                    cor.g -= x;


                GetComponent<Renderer>().material.color = cor;


                yield return new WaitForSeconds(0.1f);
            }
        }







    } 
}
