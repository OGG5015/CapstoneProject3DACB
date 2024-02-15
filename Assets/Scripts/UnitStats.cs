using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    // Stats
    public double hp = 0;
    public double attack = 0;
    public double magic = 0;
    public double defense = 0;
    public double spirit = 0;
    public double speed = 0;
    public double tier = 0;
    public double range = 0;
    public int factionTrait = 0;
    public int classTrait = 0;

    // send damage value to enemy (a rate?)
    public double damageEnemy()
    {

    }

    // calculate hp based on a draining rate sent by enemy and time
    // if hp = 0 b4 time = 0, unitDies()
    public void calculateHP()
    {

    }

    // delete the unit, I assume
    public void unitDies()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
