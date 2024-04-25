using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitBonus : MonoBehaviour
{
    //public Unit script;
    public int BonusValue { get; set; }

    public TraitBonus(int bonusValue)
    {
        this.BonusValue = bonusValue;
        Debug.Log("New stat bonus initiated");
    }
}
