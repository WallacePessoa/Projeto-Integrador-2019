using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Rank : ScriptableObject
{
    public List<int> Ranking = new List<int>();
    public List<string> Nomes = new List<string>();
    public List<int> ListAux = new List<int>();
    int aux = 0;
    string AuxNome;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<int> PrimeirosColocados()
    {
        return Ranking;
    }

    public string Chamar(int x)
    {   
        return Nomes[x] + "   Score: " + Ranking[x].ToString() + "\n";
    }

    public void AddRank(int score)
    {
        Ranking.Add(score);

            ListAux = Ranking;






        for(int y = 0; y < ListAux.Count; y++)
        {

            for (int x = 0; x < ListAux.Count; x++)
            {
                if(ListAux[y] >= ListAux[x])
                {
                    aux = ListAux[x];
                    ListAux[x] = ListAux[y];
                    ListAux[y] = aux;

                    if(Nomes.Count > x && Nomes.Count > y)
                    {
                        AuxNome = Nomes[x];
                        Nomes[x] = Nomes[y];
                        Nomes[y] = AuxNome;
                    }


                }

            }
        }



    }
}
