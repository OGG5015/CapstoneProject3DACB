using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseStats : MonoBehaviour
{
    public List<StatBonus> BaseAdditive { get; set; }
    public int BaseValue { get; set; }
    public string StatName { get; set; }
    public string StatDescription { get; set; }
    public int FinalValue { get; set; }

    public BaseStats(int baseValue, string statName, string statDescription)
    {
        this.BaseAdditive = new List<StatBonus>();
        this.BaseValue = baseValue;
        this.StatName = statName;
        this.StatDescription = statDescription;
    }
    public void AddStatBonus(StatBonus statBonus)
    {
        this.BaseAdditive.Add(statBonus);
    }
    public void RemoveStatBonus(StatBonus statBonus)
    {
        this.BaseAdditive.Remove(statBonus);

    }
    public int GetCalculatedStatValue()
    {
        this.BaseAdditive.ForEach(x => this.FinalValue += x.BonusValue);
        FinalValue += BaseValue;
        return FinalValue;
    }
}
