using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    AudioSource Audio;
    Animator AnimTextScore;
    public Animator Animator;

    Vector3 PosicaoVertcalAlvo;
    Vector3 posicaoAlvo;

    RectTransform Rect;

    bool atv = false;

    int LaneAtual = -5;
    int LaneAlvo = -5;

    public float Jump;
    public float speed;
    public Text Score;
    public Text TextDead;
    public Text TextWin;
    public Image[] vida;
    public Image Fade;
    public InputField Nome;
    public Button Salvar;

    public Rank rank;
    public Running running;

    public AudioSource audioMoeda;


    int score = 0;
    public static int Life = 3;

    List<string> Nomes = new List<string>();

    //public float Velocity = 5;

    //public GameObject TargetFinish;

    //public GameObject Parede;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Audio = GetComponent<AudioSource>();

        AnimTextScore = Score.GetComponent<Animator>();
        AnimTextScore.enabled = !AnimTextScore.enabled;

        PosicaoVertcalAlvo.x = -5;
        Score.text = score.ToString();

        StartCoroutine(Muv());
    }


    public IEnumerator Muv()
    {

        if (Input.GetKey(KeyCode.D))
        {
            MudarLane(true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            MudarLane(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Animator.SetBool("Slide", true);
        }else
            Animator.SetBool("Slide", false);

        if (Input.GetKeyDown(KeyCode.W)&& atv == false)
        {
            atv = true;
            Animator.SetBool("Jump", true);
            rb.AddForce(transform.up * Jump);
            yield return new WaitForSeconds(1f);
            atv = false;
        }
        else
        {
            Animator.SetBool("Jump", false);

        }



        yield return null;
        StartCoroutine(Muv());
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        posicaoAlvo = new Vector3(PosicaoVertcalAlvo.x, transform.position.y, transform.position.z);
        transform.position = posicaoAlvo;

        PosicaoVertcalAlvo = new Vector3(LaneAtual, 0, 0);

        if(ObjectsMusic.timeAudio >= 122f)
        {
            StartCoroutine(FadeWin());
        }
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {

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
                Animator.SetBool("Dead", true);
                StartCoroutine(FadeDead());
                Fade.gameObject.SetActive(true);
                running.Velocity = 0;

            }

        }
        if (other.CompareTag("Coletavel"))
        {
            Rect = Score.GetComponent<RectTransform>();
            Rect.localScale.Set(1, 1, 1);
            score += 1;
            Score.text = score.ToString();
            audioMoeda.Play();
            Destroy(other.gameObject);

            if(Score.fontSize == 25)
            {
                Score.fontSize = 40;
                yield return new WaitForSeconds(0.05f);
                Score.fontSize = 25;
            }


        }
    }

    public IEnumerator FadeDead()
    {
        yield return new WaitForSeconds(2f);
        Nome.gameObject.SetActive(true);
        Salvar.gameObject.SetActive(true);
        TextDead.gameObject.SetActive(true);
    }
    public IEnumerator FadeWin()
    {
        yield return new WaitForSeconds(2f);
        Nome.gameObject.SetActive(true);
        Salvar.gameObject.SetActive(true);
        TextWin.gameObject.SetActive(true);
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
                {
                    LaneAlvo += 5;
                    //if(transform.position)
                    //rb.velocity = transform.right * Velocity;
                }
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
