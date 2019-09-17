using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int score = 0;

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
