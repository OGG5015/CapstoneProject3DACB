using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI : MonoBehaviour{
    //AI steps
    //1: scan for nearest enemy
    //2: if nearest enemy in range, attack
    //2.5: if nearest enemy not in range, move towards nearest enemy
    //3: repeat
    //x & y keep track of units current position on board

    //int x;
    //int y;
    //int nearestx;
    //int nearesty;
    bool toggle; //toggles if combat is active or not
    private ArrayList unitList = new ArrayList();

    //Scan returns true if there is an enemy in range
    bool Scan(int unit){




        return false;
    }

    //returns the number of the unit that is the closest
    int getNearest(int unit){


        return 0;
    }

    //returns the distance between two units
    int getDistance(int unit, int enemy) {
        

        return 0;
    }

    void Attack(int unit, int enemy){
        /*if (unitList[unit].isPhys()) {
            unitList[enemy].updateHealth(unitList[unit].getStr() - unitList[enemy].getDef());
        }
        else{
            unitList[enemy].updateHealth(unitList[unit].getMag() - unitList[enemy].getSpr());
        }*/
    }

    //Move moves the unit to an adjacent tile, updating x & y as well
    void Move(int unit, int enemy){ 
        
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    void toggleToggle() {
        toggle = !toggle;
    }

    // Update is called once per frame
    void Update(){
        if(toggle){
            for (int unit = 0; unit < unitList.Count; i++){
                if (Scan(unit)){
                    Attack(unit, getNearest(unit));
                }
                else {
                    Move(unit, getNearest(unit));
                }
            }
        }
    }
}
