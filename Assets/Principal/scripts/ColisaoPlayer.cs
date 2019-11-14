using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColisaoPlayer : MonoBehaviour
{


    Animator AnimTextScore;
    Animator Animator;

    RectTransform Rect;


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



    void Start()
    {
        Animator = GetComponent<Animator>();
        AnimTextScore = Score.GetComponent<Animator>();
        AnimTextScore.enabled = !AnimTextScore.enabled;

    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectsMusic.timeAudio >= 122f)
        {
            StartCoroutine(FadeWin());
        }
    }



    private IEnumerator OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Dead"))
        {
            Life--;

            for (int x = 0; x < vida.Length; x++)
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

            if (Score.fontSize == 25)
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

}
