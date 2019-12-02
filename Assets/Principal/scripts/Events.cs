using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Events : MonoBehaviour
{
    public void clickGetJogador()
    {
        StartCoroutine(getJogador());
    }
    public void clickGetJogadores()
    {
        StartCoroutine(getJogadores());
    }
    public void clickPostJogador()
    {

    }

    IEnumerator getJogador()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:44388/api/Jogador/1");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }

        Debug.Log(www.downloadHandler.text);

        string json = www.downloadHandler.text;
        var playerToJson = JsonUtility.FromJson<teste>(json);
        Debug.Log(playerToJson);

    }

    static public IEnumerator postJogador(int Id, string Nome, int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", Id);
        form.AddField("nome", Nome);
        form.AddField("score", score);
        UnityWebRequest www = UnityWebRequest.Post("https://localhost:44388/api/Jogador", form);
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {//executou corretamente
            Debug.Log(www);
        }

    }

    IEnumerator getJogadores()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:44388/api/Jogador");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }

        Debug.Log(www.downloadHandler.text);

        string json = www.downloadHandler.text;
        var playerToJson = JsonHelper.FromJson<teste>(json);
        Debug.Log(playerToJson);

    }
}
