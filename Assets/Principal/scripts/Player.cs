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

    int LaneAtual = 5;
    int LaneAlvo;

    public float Jump;
    public float speed;
    public Text Score;
    public Image[] vida;
    public Rank rank;
    int score = 0;
    int Life = 3;

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region muv

        //transform.LookAt(TargetFinish.transform.position);

        //rb.velocity = transform.forward * Velocity;

        if (Input.GetKeyDown(KeyCode.D) && atv == false){
            atv = true;
            MudarLane(5);
        }
        if(Input.GetKeyUp(KeyCode.D))
            atv = false;

        if (Input.GetKeyDown(KeyCode.A) && atv == false)
        {
            atv = true;
            MudarLane(-5);
        }
        if (Input.GetKeyUp(KeyCode.A))
            atv = false;

        posicaoAlvo = new Vector3(PosicaoVertcalAlvo.x, transform.position.y, transform.position.z);
        transform.position = posicaoAlvo;
        //transform.position = Vector3.MoveTowards(transform.position, posicaoAlvo, speed);

        #endregion




        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up* Jump);
           
        }



        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //    Instantiate(Parede, transform.position, transform.rotation);
        //}

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
                //rank.AddRank(score);
                SceneManager.LoadScene("Menu1");
            }

        }
    }


    void MudarLane(int Lane)
    {

        LaneAlvo = LaneAtual + Lane;
        if (LaneAlvo < -6 || LaneAlvo > 6)
            return;
        LaneAtual = LaneAlvo;

        //print(LaneAlvo);

        PosicaoVertcalAlvo = new Vector3(( LaneAtual - 5 ) , 0, 0);
    }
}
