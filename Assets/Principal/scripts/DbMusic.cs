using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbMusic : MonoBehaviour
{

    public GameObject Player;

    public GameObject[] Itens;


    public float[] RefValores;

    //public float refValue = 0.1f;
    public float volume = 2;

    int qSamples = 10000;

    float rmsValue;
    float dbValue;


    float[] DbValue;

    float time;
    float[] sample;

    bool aux = false;

    AudioSource Audio;


    // Start is called before the first frame update
    void Start()
    {
        Audio = Player.GetComponent<AudioSource>();
        sample = new float[qSamples];
        DbValue = new float[Itens.Length];
        StartCoroutine(GetVolume());



    }

    public IEnumerator GetVolume()
    {

        Audio.GetOutputData(sample, 0);

        


        int i;
        float som = 0;

        time = time + Time.deltaTime;





        for (i = 0; i < qSamples; i++)
        {
            som += sample[i] * sample[i];

            if (sample[i] >= 0.8)
            {
                //print(sample[i]);
            }

        }

        rmsValue = Mathf.Sqrt(som / qSamples);

        for(int x = 0; x < Itens.Length; x++)
        {
            DbValue[x] = 20 * Mathf.Log10(rmsValue / RefValores[x]);
            if (DbValue[x] < -160)
                DbValue[x] = -160;
            print(rmsValue);
        }

        yield return null;
        StartCoroutine(GetVolume());
    }




    // Update is called once per frame
    void FixedUpdate()
    {


        for(int x = 0; x < Itens.Length; x++)
        {
            //print(volume * DbValue[x]);
            Itens[x].transform.localScale = new Vector3(Itens[x].transform.localScale.x, Itens[x].transform.localScale.y, volume * DbValue[x]);
        }


        // transform.localScale = new Vector3(transform.localScale.x, volume * dbValue, transform.localScale.z);
        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, volume * dbValue);
        //transform.position = new Vector3(transform.position.x, transform.position.y, Player.transform.position.z);
        //print(volume * rmsValue);



    }
}
