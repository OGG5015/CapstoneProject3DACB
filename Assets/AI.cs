using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    private bool requireTarget;
    private int target;
    private Vector3 closestTarget;
    private int distance;

    //Scan returns true if there is an enemy in range
    bool Scan(int unit){
        //hexGrid = GetComponentInParent<HexGrid>();

        /*if(hexGrid == null)
        {
            hexGrid = GetComponentInParent<HexGrid>();
        }*/

        /*MouseController.instance.OnLeftMouseClick += OnLeftMouseClick;
        MouseController.instance.OnRightMouseClick += OnRightMouseClick;*/

        /*centrePosition.x = (x) * (OuterRadius(hexSize) * 1.5f);
        centrePosition.y = 0f;
        centrePosition.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);*/

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

            //AIFunctionality();

            for (int unit = 0; unit < unitList.Count; unit++){
                if (Scan(unit)){
                    Attack(unit, getNearest(unit));
                }
                else {
                    Move(unit, getNearest(unit));
                }
            }
        }
    }

    //nearestInRange returns true if nearest enemy unit is within range
    /*bool nearestInRange(int unit){
        if ((unitList[unit].getRange() > 0) && (Vector3.Distance(transform.position, target.position) > unitList[unit].getRange())){
            return false;
        }
        else{
            return true;
        }
    }*/
    void AIFunctionality(int unit){
        if ((!requireTarget)){
            return; //if no target was set and we require one, AI will not function.
        }

        Vector3 moveToward = closestTarget - transform.position; //Used to face the AI in the direction of the target
        Vector3 moveAway = transform.position - closestTarget; //Used to face the AI away from the target when running away
        //float distance = Vector3.Distance(transform.position, target.position);

        if (requireTarget){
            Attack(unit, target);
        }
        /*else if (nearestInRange()){
            if (!toggle){
                return;
            }
            if (distance > unitList[unit].getRange()){
                canAttack = false; //the target is too far away to attack
                Move(unit, getNearest(unit)); //move closer
            }
        }*/
            //start attacking if close enough

        /*if ((distance < unitList[unit].getRange())){
            Attack(unit, target);
        }*/
    }
}

