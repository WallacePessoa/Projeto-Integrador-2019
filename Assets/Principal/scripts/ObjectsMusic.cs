using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectsMusic : MonoBehaviour
{

    public GameObject Parede;
    public GameObject Destruir;
    public GameObject Coletavel;
    //public GameObject Inimigo;


    public List<GameObject> Objects = new List<GameObject>();

    public List<float> TimeColetaveis = new List<float>();


    public List<float> Tempos = new List<float>();
    public List<float> Amostras = new List<float>();

    public GameObject[] ItensDb;

    public float[] RefValores;
    public float[] valores;
    public float[] times;
    public Quaternion rotação;

    public float volume = 2;
    public float DistanciaObject;
    public float DistanciaObject2;
    public float StartMusicTime;

    public Text StarMusicText;

    int qSamples = 16384;
    int aux = 0;
    int Rnd;
    int Rnd2;
    public static float timeAudio;

    public static float[] DbValue;
    float[] SampleScript;
    float[] SampleMusic;

    float rmsValue;
    float dbValue;
    float TimeBeatStart = 0;
    float TimeBeat = 0;
    float TimeColet = 0;
    float TimeEnemi = 0;
    float som = 0;
    float contagemTempo;
    float TempoMusica;


    bool starTime = false;
    bool Aux = false;

    bool anima = true;

    bool instantiate = true;
    bool instantiateColet = true;
    bool instantiateEnemi = false;

    bool InstaciarObjeto = true;

    public AudioSource AudioScript;

    AudioSource AudioMusic;

    Animator animator;


    GameObject Obstaculo;
    GameObject col;
    GameObject Enemi;

    void Start()
    {
        StarMusicText.text = StartMusicTime.ToString();

        rotação = Quaternion.Euler(new Vector3(-90, 0, 0));

        animator = GetComponent<Animator>();
        AudioMusic = GetComponent<AudioSource>();


        SampleScript = new float[qSamples];
        SampleMusic = new float[qSamples];

        DbValue = new float[ItensDb.Length];

        valores = new float[11];

        times = new float[10];

        animator.enabled = !animator.enabled;


        for (int x = 0; x < valores.Length; x++)
        {
            valores[x] = 0.01f;
        }
        StartCoroutine(IniciarMusica());
    }

    public IEnumerator GetVolume()
    {

        AudioScript.GetOutputData(SampleScript, 0);
        AudioMusic.GetOutputData(SampleMusic, 0);


        som = 0;

        for (int i = 0; i < qSamples; i++)
        {
            som += SampleMusic[i] * SampleMusic[i];

            /*

            if (InstaciarObjeto && SampleScript[i] > valores[1] - 0.03)
            {
                InstaciarObjeto = false;
                Rnd = Random.Range(0, 3);
                if (Rnd == 0)
                    Obstaculo = Instantiate(Parede, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

                if (Rnd == 1)
                    Obstaculo = Instantiate(Parede, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

                if (Rnd == 2)
                    Obstaculo = Instantiate(Parede, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);
                Obstaculos.Add(Obstaculo);

                TimeBeatStart = 0;
            }
                        */



            if (instantiateColet && SampleScript[i] > 0.3)
            {
                if (Rnd == 0)
                    col = Instantiate(Coletavel, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

                if (Rnd == 1)
                    col = Instantiate(Coletavel, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

                if (Rnd == 2)
                    col = Instantiate(Coletavel, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);
                instantiateColet = false;
            }


                       
            //if (instantiateEnemi && SampleScript[i] > 0.35)
            //{

            //    if (Rnd == 0)
            //        Enemi = Instantiate(Inimigo, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

            //    if (Rnd == 1)
            //        Enemi = Instantiate(Inimigo, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

            //    if (Rnd == 2)
            //        Enemi = Instantiate(Inimigo, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);
            //    instantiateEnemi = false;
            //}




            if (aux < 10)
            {

                if (SampleScript[i] > valores[aux])
                {
                    valores[aux] = SampleScript[i];



                    Amostras.Add(valores[aux]);
                    Tempos.Add(AudioScript.time);

                    if (aux == 0)
                    {


                        times[0] = AudioScript.time;
                    }

                    if (aux == 1)
                    {
                        times[1] = AudioScript.time;
                    }

                    if (aux == 2)
                    {
                        times[2] = AudioScript.time;

                        if (valores[1] < valores[2] || valores[1] - 0.05 < valores[2])
                        {

                            TimeBeat = times[2] - times[1];
                            //print(TimeBeat);

                        }
                    }
                }
            }



            rmsValue = Mathf.Sqrt(som / qSamples);

            for (int x = 0; x < ItensDb.Length; x++)
            {
                DbValue[x] = 20 * Mathf.Log10(rmsValue / RefValores[x]);
                if (DbValue[x] < -160)
                    DbValue[x] = -160;

            }
        }
        timeAudio = AudioMusic.time;
        yield return null;
        StartCoroutine(GetVolume());
    }



    public IEnumerator IniciarMusica()
    {

        yield return new WaitForSeconds(1f);
        starTime = true;
        AudioScript.Play();
        StartMusicTime--;
        StarMusicText.text = StartMusicTime.ToString();
        if(StartMusicTime == 0)
        {
            AudioMusic.Play();
            StarMusicText.enabled = !StarMusicText.enabled;


        }
        else
            StartCoroutine(IniciarMusica());
        StartCoroutine(GetVolume());
    }


    void Update()
    {


        //if (AudioMusic.time >= 122f)
        //{
        //    SceneManager.LoadScene("Menu1");
        //}

        if (InstaciarObjeto && Input.GetKey(KeyCode.I))
        {

            InstaciarObjeto = false;
            Objects.Add(Obstaculo);
        }
        if (Input.GetKey(KeyCode.O) && instantiateColet)
        {
            if (Rnd == 0)
                col = Instantiate(Coletavel, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

            if (Rnd == 1)
                col = Instantiate(Coletavel, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

            if (Rnd == 2)
                col = Instantiate(Coletavel, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);
            Objects.Add(col);
            TimeColetaveis.Add(AudioMusic.time);
            instantiateColet = false;
        }
        //if (instantiateEnemi && Input.GetKey(KeyCode.P))
        //{
        //    if (Rnd == 0)
        //        Enemi = Instantiate(Inimigo, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

        //    if (Rnd == 1)
        //        Enemi = Instantiate(Inimigo, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);

        //    if (Rnd == 2)
        //        Enemi = Instantiate(Inimigo, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject), transform.rotation);
        //    Objects.Add(Enemi);
        //    instantiateEnemi = false;
        //}

        

        #region Start Time
        if (starTime)
        {

            if (anima)
            {
                anima = false;
                animator.enabled = !animator.enabled;
            }



            if (TimeBeat == 0)
            {
                if (TimeBeatStart < 1)
                {

                    TimeBeatStart += Time.deltaTime;

                }
                else
                {
                    TimeBeatStart = 0;
                    aux++;
                    InstaciarObjeto = true;
                }

            }
            else
            {

                TimeBeatStart += Time.deltaTime;

                if (TimeBeatStart > TimeBeat && TimeBeat > 1)
                {
                    aux++;
                    InstaciarObjeto = true;


                }
                else {

                    InstaciarObjeto = false;
                }



            }

            TimeColet += Time.deltaTime;
            if (TimeColet > 0.5)
            {
                TimeColet = 0;
                instantiateColet = true;
            }

            TimeEnemi += Time.deltaTime;
            if (TimeEnemi > 1)
            {
                TimeEnemi = 0;
                instantiateEnemi = true;
            }

            
        }
        #endregion

        for (int x = 0; x < ItensDb.Length; x++)
        {
            ItensDb[x].transform.localScale = new Vector3(ItensDb[x].transform.localScale.x, ItensDb[x].transform.localScale.y, volume * DbValue[x]);
        }


    }



    public void InstParede()
    {
        Rnd2 = Random.Range(0, 2);
        Rnd = Random.Range(0, 3);

        print(Rnd2);
        if (Rnd2 == 0)
        {

            if (Rnd == 0)
                Obstaculo = Instantiate(Parede, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

            if (Rnd == 1)
                Obstaculo = Instantiate(Parede, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

            if (Rnd == 2)
                Obstaculo = Instantiate(Parede, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);
        }
        else
        {
            if (Rnd == 0)
                Obstaculo = Instantiate(Parede, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

            if (Rnd == 1)
                Obstaculo = Instantiate(Parede, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

            if (Rnd == 2)
                Obstaculo = Instantiate(Parede, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);
        }



    }

    public void InsColet()
    {
        if (Rnd == 0)
            col = Instantiate(Coletavel, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

        if (Rnd == 1)
            col = Instantiate(Coletavel, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

        if (Rnd == 2)
            col = Instantiate(Coletavel, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);
    }

    //public void InstEnemi()
    //{
    //    if (Rnd == 0)
    //        Enemi = Instantiate(Inimigo, new Vector3(-10, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

    //    if (Rnd == 1)
    //        Enemi = Instantiate(Inimigo, new Vector3(-5, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);

    //    if (Rnd == 2)
    //        Enemi = Instantiate(Inimigo, new Vector3(0, transform.position.y, transform.position.z + DistanciaObject2), transform.rotation);
    //}



}

