using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Post : MonoBehaviour
{
    ChromaticAberration ca;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for(int x=0;x< ObjectsMusic.DbValue.Length; x++)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.TryGetSettings(out ChromaticAberration ca);
            print(ObjectsMusic.DbValue[x]);
            ca.intensity.value = Mathf.Abs(ObjectsMusic.DbValue[x]) / 10;
        }


    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Parede"))
        //{
        //    if(ca.intensity.value != 1)
        //    {
        //    ca.intensity.value = 1;
        //    yield return new WaitForSeconds(0.1f);
        //    ca.intensity.value = 0;
        //    }
        //}

        yield return null;
    }
}
