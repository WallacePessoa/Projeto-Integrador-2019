using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRank : MonoBehaviour
{

    Text Text;
    public Rank rank;

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<Text>();
        //Text.text = rank.Chamar();



        for(int x = 0; x < rank.Ranking.Count; x++)
        {
            Events.postJogador(x + 1, rank.Nomes[x], rank.Ranking[x]);
            Text.text =Text.text + (x+1) + "  " + "Nome: "+ rank.Chamar(x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
