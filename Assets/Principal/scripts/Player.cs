using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    AudioSource Audio;

    Vector3 PosicaoVertcalAlvo;
    Vector3 posicaoAlvo;

    bool atv = true;

    int LaneAtual = -5;
    int LaneAlvo = -5;

    public float Jump;
    public float speed;
    public Text Score;
    public Text Text;
    public Image[] vida;
    public Image Fade;
    public InputField Nome;
    public Button Salvar;

    public Rank rank;
    public Running running;


    int score = 0;
    int Life = 3;

    List<string> Nomes = new List<string>();

    //public float Velocity = 5;

    //public GameObject TargetFinish;

    //public GameObject Parede;




  



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Audio = GetComponent<AudioSource>();

        PosicaoVertcalAlvo.x = -5;
        Score.text = score.ToString();

        StartCoroutine(Muv());
    }


    public IEnumerator Muv()
    {

        if (Input.GetKey(KeyCode.D))
        {
            atv = true;
            MudarLane(true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            atv = true;
            MudarLane(false);
        }


        yield return null;
        StartCoroutine(Muv());
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        posicaoAlvo = new Vector3(PosicaoVertcalAlvo.x, transform.position.y, transform.position.z);
        transform.position = posicaoAlvo;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up* Jump);
           
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //    Instantiate(Parede, transform.position, transform.rotation);
        //}

        PosicaoVertcalAlvo = new Vector3(LaneAtual, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coletavel"))
        {
            score += 1;
            Score.text = score.ToString();
            Destroy(other.gameObject);

        }
        if (other.CompareTag("Dead"))
        {
            Life--;

            for(int x = 0; x < vida.Length; x++)
            {
                if (vida[x].enabled == true)
                {
                    vida[x].enabled = !vida[x].enabled;
                    break;
                }
            }


            if (Life == 0)
            {
                StartCoroutine(FadeDead());
                Fade.gameObject.SetActive(true);
                running.Velocity = 0;
            }

        }
    }

    public IEnumerator FadeDead()
    {
        yield return new WaitForSeconds(2f);
        Nome.gameObject.SetActive(true);
        Salvar.gameObject.SetActive(true);
        Text.gameObject.SetActive(true);
    }

    public void Gravar()
    {
        rank.Nomes.Add(Nome.text);
        rank.AddRank(score);
        SceneManager.LoadScene("Menu1");
    }


    void MudarLane(bool Lane)
    {
        switch (LaneAtual)
        {
            case -5:
                if (Lane)
                    LaneAlvo += 5;
                if (!Lane)
                    LaneAlvo -= 5;
                break;
            case 0:
                if (!Lane)
                    LaneAlvo -= 5;
                break;
            case -10:
                if (Lane)
                    LaneAlvo += 5;
                break;
        }
        LaneAtual = LaneAlvo;
    }
}
