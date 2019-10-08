using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Rank : ScriptableObject
{
    public List<int> Ranking = new List<int>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRank(int score)
    {
        Ranking.Add(score);
    }
}
