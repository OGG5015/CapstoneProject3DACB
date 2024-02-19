using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit{
    private int xCoord;
    private int yCoord;
    private int nearX;
    private int nearY;

    private int maxHp;
    private int currHp;
    private int str;
    private int mag;
    private int def;
    private int spr;
    private int spd;
    private int tier;
    private int range;
    private bool phys;
    private string trait1;
    private string trait2;
    //private ArrayList traitList;

    public Unit(int maxHp, int str, int mag, int def, int spr, int spd, int tier, int range, bool phys, string trait1, string trait2)
    {
        this.maxHp = maxHp;
        this.currHp = maxHp;
        this.str = str;
        this.mag = mag;
        this.def = def;
        this.spr = spr;
        this.spd = spd;
        this.tier = tier;
        this.range = range;
        this.phys = phys; //if a unit is a physical unit
        this.trait1 = trait1;
        this.trait2 = trait2;
        //this.traitList = new ArrayList();
        //traitList.Add(trait1);
        //traitList.Add(trait2);
    }

    public int getMaxHp() { return this.maxHp; }
    public int getStr() { return this.str; }
    public int getMag() { return this.mag; }
    public int getDef() { return this.def; }
    public int getSpr() { return this.spr; }
    public int getSpd() { return this.spd; }
    public int getTier() { return this.tier; }
    public int getRange() { return this.range; }
    public bool isPhys() { return this.phys; }

    //updates the x&y coordinates
    public void updateCoords() {
        
    }

    public int getX() { return this.xCoord; }
    public int getY() { return this.yCoord;}

    //updates nearX & nearY to the x & y coordinate of the nearest enemy
    public void updateClosest() {
        
    }

    //unit takes damage
    public void updateHealth(int dmg) { this.currHp -= dmg; }
}
