using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Rank : ScriptableObject
{
    public List<int> Ranking = new List<int>();
    int aux = 0;
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

    public void AddRank(int score)
    {
        Ranking.Add(score);
        Ranking.Sort();


    }
}
