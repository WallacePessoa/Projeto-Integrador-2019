using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject scene;
    public GameObject posIni;
    public GameObject Camera;


    GameObject inst;

    float posZmin;

    int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        posZmin = 0f;
        // print("posi em Z" + posZmin);
        inst = Instantiate(scene, new Vector3(-5f, 0f, posZmin), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.transform.position.z >= posZmin-170)
        {
            Destroy(inst);
            StartCoroutine("InstantiateScene");
        }
    }

    /*void InstantiateScene()
    {
        Destroy(scene, 8);
        posZmin -= 212.5616f;

        //posIni.transform.localPosition = new Vector3(0, 0, posZmin);

        //Instantiate(scene, posIni.transform);

        Instantiate(scene, new Vector3(-27.92109f, -13.50165f, posZmin), Quaternion.identity);
    }*/

    IEnumerator InstantiateScene()
    {
        //print(posZmin);
        posZmin += 482.4f;
        inst = Instantiate(scene, new Vector3(-5f, 0f, posZmin), Quaternion.identity);
        yield return new WaitForSeconds(0f);




    }
}
