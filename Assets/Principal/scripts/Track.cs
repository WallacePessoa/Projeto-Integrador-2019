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
        posZmin = 83;
        inst = Instantiate(scene, new Vector3(-4.3f, 0f, posZmin), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.transform.position.z >= posZmin+30)
        {
            Destroy(inst);
            StartCoroutine("InstantiateScene");
        }
    }


    IEnumerator InstantiateScene()
    {
        //print(posZmin);
        posZmin += 167;
        inst = Instantiate(scene, new Vector3(-4.3f, 0.5f, posZmin), Quaternion.identity);
        yield return new WaitForSeconds(0f);




    }
}
