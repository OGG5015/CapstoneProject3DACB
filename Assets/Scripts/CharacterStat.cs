
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public List<BaseStats> stats = new List<BaseStats>();
    // Start is called before the first frame update
    void Start()
    {
        stats.Add(new BaseStats(4, "Speed", "Your Power Level"));
        stats[0].AddStatBonus(new StatBonus(5));
        Debug.Log(stats[0].GetCalculatedStatValue());
        
    }

   
}
