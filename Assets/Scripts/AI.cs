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
    //pass by reference not copy

    //int x;
    //int y;
    //int nearestx;
    //int nearesty;

    public static int maxHp = 40;
    public int currHp = maxHp;
    public int str = 15;
    public int mag = 15;
    public int def = 10;
    public int spr = 10;
    public int spd = 10;
    public int tier = 1;
    public float range = 1.0f;
    public int price = 0;
    public float drop = 0;

    public float boostRange = 5.0f;

    public float boostAmount = 5.0f;
    public bool phys = true;
    private string trait1;
    private string trait2;
    public int team = 1;
    public bool sFight = false;

    public bool combat = true; //toggles if combat is active or not
    public bool move = true; //toggles if unit should move or not
    private ArrayList unitList = new ArrayList();
    private bool requireTarget;
    private Vector3 closestTarget;
    private int distance;
    
    public GameObject nearestEnemy;
    public float speed = 5.0f;

    //private float size;

    private Vector3 target;

    private HexGrid hexGrid;
    private UnitBench unitBench;
    Vector3 mousePosition;
    bool isDragging = false;

    public int GetMaxHP() // had to add bc its static
    {
        return maxHp;
    }

    //returns the number of the unit that is the closest
    int getNearest(int unit){


        return 0;
    }

    //returns the distance between two units
    int getDistance(int unit, int enemy) {
        

        return 0;
    }

    public void setHp(int dmg) {
        currHp -= dmg;
    }

    public void fight() {
        if (phys){
            nearestEnemy.GetComponent<AI>().setHp(str - nearestEnemy.GetComponent<AI>().def);
        }
        else{
            nearestEnemy.GetComponent<AI>().setHp(mag - nearestEnemy.GetComponent<AI>().spr);
        }
    }

    void OnTriggerEnter(Collider col)
    {//need to tag objects with teamA or teamB
        if (GetComponent<Collider>().CompareTag("teamB"))
        {
            move = false;
            sFight = true;
        }
    }

    //Move moves the unit to an adjacent tile, updating x & y as well
    void Move(int unit, int enemy){ 
        
    }

    // Start is called before the first frame update
    void Start(){
        nearestEnemy = FindClosestEnemy();
        target = nearestEnemy.transform.position;
        hexGrid = this.GetComponent<Collider>().GetComponentInParent<HexGrid>();
    }

    public void toggleCombat() {
        combat = !combat;
        Debug.Log("Combat is " +  combat);
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;

        if (gameObject.tag == "T1") {
            gos = GameObject.FindGameObjectsWithTag("T2");
        }
        else
        {
            gos = GameObject.FindGameObjectsWithTag("T1");
        }
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    /*
     * public GameObject FindClosestEnemy()
    {
        GameObject[] gos;

        if (gameObject.tag == "T1") {
            gos = GameObject.FindGameObjectsWithTag("T2");
        }
        else
        {
            gos = GameObject.FindGameObjectsWithTag("T1");
        }
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
     */

    bool inRange() {

        //Debug.Log("Calls inRange()");
        if ((gameObject.transform.position - nearestEnemy.transform.position).sqrMagnitude <= ((hexGrid.HexSize*8f) * range))//was 20f 
        {
            return true;
        }

        return false;
    }
    //Trait bonuses
           public void BoostTraitsWithinRange()
    {
          foreach (GameObject unit in unitList)
        {
             if (unit.GetComponent<AI>().team == this.team)
            {
                foreach (GameObject otherUnit in unitList)
                {
                    if (otherUnit == unit || otherUnit.GetComponent<AI>().team != this.team)
                        continue;

                    float distance = Vector3.Distance(unit.transform.position, otherUnit.transform.position);

                    if (distance <= boostRange)
                    {
                        AI unitAI = unit.GetComponent<AI>();
                        AI otherUnitAI = otherUnit.GetComponent<AI>();

                        unitAI.str += (int)boostAmount;
                        unitAI.mag += (int)boostAmount;
                        unitAI.def += (int)boostAmount;
                        unitAI.spr += (int)boostAmount;
                        unitAI.spd += (int)boostAmount;

                        otherUnitAI.str += (int)boostAmount;
                        otherUnitAI.mag += (int)boostAmount;
                        otherUnitAI.def += (int)boostAmount;
                        otherUnitAI.spr += (int)boostAmount;
                        otherUnitAI.spd += (int)boostAmount;
                     } } }
                }
                }


    // Update is called once per frame
    void Update(){
        //SnapToHexCenter();
        Debug.Log("Name: " + gameObject.name);
        Debug.Log("gameObject postion: " + gameObject.transform.position);
        Debug.Log("target postion: " + target);
        Debug.Log("nearestEnemy position: " + nearestEnemy.transform.position);
        

        if (currHp <= 0)
        {
            Destroy(gameObject);
        }

        if (nearestEnemy == null) {
            nearestEnemy = FindClosestEnemy();
        }

        if (combat == true && nearestEnemy != null)
        {
            
            if (((gameObject.transform.position - target).sqrMagnitude <= hexGrid.HexSize) && !inRange())
            {
                move = false;
                Debug.Log("What is here?");
                //x is greater
                if (gameObject.transform.position.x > nearestEnemy.transform.position.x)
                {
                    Debug.Log("x is greater");
                    //unit is to upper right of target or on same y, move lower left
                    if (gameObject.transform.position.z >= nearestEnemy.transform.position.z)
                    {
                        target.z = target.z - hexGrid.HexSize;
                        target.x = target.x - (hexGrid.HexSize * 1.5f);
                    }
                    //unit is to lower right to target, move upper left
                    else if (gameObject.transform.position.z < nearestEnemy.transform.position.z)
                    {
                        target.z = target.z + hexGrid.HexSize;
                        target.x = target.x - (hexGrid.HexSize * 1.5f);
                    }
                }
                //x is less
                else if (gameObject.transform.position.x < nearestEnemy.transform.position.x)
                {
                    Debug.Log("x is less");
                    //unit is to upper left of target, move lower right
                    if (gameObject.transform.position.z > nearestEnemy.transform.position.z)
                    {
                        target.z = target.z - hexGrid.HexSize;
                        target.x = target.x + (hexGrid.HexSize * 1.5f);
                    }
                    //unit is to lower left of target or same y, move upper right
                    else if (gameObject.transform.position.z <= nearestEnemy.transform.position.z)
                    {
                        target.z = target.z + hexGrid.HexSize;
                        target.x = target.x + (hexGrid.HexSize * 1.5f);
                    }
                }
                //x is equal
                else 
                {
                    Debug.Log("x is equal");
                    //unit is above target, move down
                    if (gameObject.transform.position.z > nearestEnemy.transform.position.z)
                    {
                        target.z = target.z - (hexGrid.HexSize * 1.5f);
                    }
                    //unit is below target, move up
                    else if (gameObject.transform.position.z < nearestEnemy.transform.position.z)
                    {
                        target.z = target.z + (hexGrid.HexSize * 1.5f);
                    }
                }

                if(!inRange()){move = true;}

                
            }
            else { Debug.Log("Math: " +((gameObject.transform.position - target).sqrMagnitude) + " ? " + hexGrid.HexSize); }
            
            if (inRange())
                {
                    fight();
                }

            if (move)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed);
            }
            //Calling trait bonus
            //BoostTraitsWithinRange(); 

            /*if (move) {

                self.transform.position = Vector3.MoveTowards(self.transform.position, nearestEnemy.transform.position, speed);
            }*/
        }

        if (gameObject.transform.position != target && nearestEnemy == null) 
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed);
        }
    }

    void AIFunctionality(int unit){
        if ((!requireTarget)){
            return; //if no target was set and we require one, AI will not function.
        }

        Vector3 moveToward = closestTarget - transform.position; //Used to face the AI in the direction of the target
        Vector3 moveAway = transform.position - closestTarget; //Used to face the AI away from the target when running away
        //float distance = Vector3.Distance(transform.position, target.position);

        if (requireTarget){
            fight();
        }
    }
}

