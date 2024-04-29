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
    public GameObject timer;
    public int currHp = maxHp;
    public int str = 15;
    public int mag = 15;
    public int def = 10;
    public int spr = 10;
    public int spd = 10;
    public int tier = 1;
    public float range = 1.0f;

    public float boostRange = 5.0f;

    public float boostAmount = 5.0f;
    public bool phys = true;
    private string trait1;
    private string trait2;
    public int team = 1;
    public bool sFight = false;

    public bool combat; //toggles if combat is active or not
    public bool move = true; //toggles if unit should move or not
    private ArrayList unitList = new ArrayList();
    private bool requireTarget;
    private Vector3 closestTarget;
    private int distance;
    
    public GameObject nearestEnemy;
    public float speed = .1f;

    //private float size;

    private Vector3 target;

    private HexGrid hexGrid;
    private UnitBench unitBench;
    Vector3 mousePosition;
    bool isDragging = false;

    //returns the number of the unit that is the closest
    int getNearest(int unit){


        return 0;
    }

    //returns the distance between two units
    int getDistance(int unit, int enemy) {
        

        return 0;
    }

    //snap stuff
    

    

    public void setHp(int dmg) {
        currHp -= dmg;
    }

    private void SnapToHexOrUnitBench()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            hexGrid = hit.collider.GetComponent<HexGrid>();
            unitBench = hit.collider.GetComponent<UnitBench>();

            if (hexGrid != null)
            {
                SnapToHexCenter();
                return;
            }
            else if (unitBench != null)
            {
                Debug.Log("Unit bench: " + unitBench);
                SnapToUnitBench();
                return;
            }
        }

        Debug.Log("Neither hex grid nor unit bench found");

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

    private void SnapToHexCenter()
    {

        Ray ray = Camera.main.ScreenPointToRay(gameObject.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {

                Vector2 offsetCoordinates = HexMetrics.CoordinateToOffset(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                offsetCoordinates = HexMetrics.AxialRound(offsetCoordinates);

                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)offsetCoordinates.x, (int)offsetCoordinates.y, hexGrid.Orientation);

                if (hexGrid.Orientation == HexOrientation.PointyTop)
                {
                    transform.position = new Vector3(center.x, transform.position.y, center.z);
                }
                else
                {
                    transform.position = center;
                }

                Debug.Log("Hex Center: " + offsetCoordinates);
                Debug.Log("Snapped to: " + transform.position);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }



    }

    private void SnapToUnitBench()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            unitBench = hit.collider.GetComponentInParent<HexGrid>().GetComponentInChildren<UnitBench>();

            if (unitBench != null)
            {

                Vector3 benchOrigin = new Vector3(
                    unitBench.transform.position.x - (unitBench.Width) * unitBench.SquareSize / 2f,
                    unitBench.transform.position.y,
                    unitBench.transform.position.z
                );

                Vector3 localPoint = hit.point;
                float distanceFromOriginX = localPoint.x - benchOrigin.x;
                int cellIndex = Mathf.FloorToInt(distanceFromOriginX / unitBench.SquareSize);

                float cellCenterX = benchOrigin.x + (cellIndex + 0.5f) * unitBench.SquareSize;
                float cellCenterZ = unitBench.transform.position.z;
                Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);

                transform.position = cellCenter;
                Debug.Log("Snapped to: " + cellCenter);
            }
            else
            {
                Debug.Log("Unit bench is null");
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }

    }

    /*
     private void SnapToUnitBench()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            unitBench = hit.collider.GetComponentInParent<HexGrid>().GetComponentInChildren<UnitBench>();

            if (unitBench != null)
            {

                Vector3 benchOrigin = new Vector3(
                    unitBench.transform.position.x - (unitBench.Width) * unitBench.SquareSize / 2f,
                    unitBench.transform.position.y,
                    unitBench.transform.position.z
                );

                Vector3 localPoint = hit.point;
                float distanceFromOriginX = localPoint.x - benchOrigin.x;
                int cellIndex = Mathf.FloorToInt(distanceFromOriginX / unitBench.SquareSize);

                float cellCenterX = benchOrigin.x + (cellIndex + 0.5f) * unitBench.SquareSize;
                float cellCenterZ = unitBench.transform.position.z;
                Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);

                transform.position = cellCenter;
                Debug.Log("Snapped to: " + cellCenter);
            }
            else
            {
                Debug.Log("Unit bench is null");
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }

    }
     */


    //Move moves the unit to an adjacent tile, updating x & y as well
    void Move(int unit, int enemy){ 
        
    }

    // Start is called before the first frame update
    void Start(){
        nearestEnemy = FindClosestEnemy();
        target = gameObject.transform.position;
        hexGrid = this.GetComponent<Collider>().GetComponentInParent<HexGrid>();
        timer = GameObject.Find("timerFill");
    }

    public void toggleCombat() {
        this.combat = !this.combat;
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

        if (timer.GetComponent<BarTimer>().isStagePlanning)
        {
            this.combat = false;
        }
        else {
            this.combat = true;
        }

        if(currHp <= 0)
        {
            Destroy(gameObject);
        }

        if (nearestEnemy == null) {
            nearestEnemy = FindClosestEnemy();
        }

        if (combat  && nearestEnemy != null)
        {
            
            if (((gameObject.transform.position - target).sqrMagnitude <= hexGrid.HexSize) && !inRange()) {
                move = false;
                
                //x is greater
                if (gameObject.transform.position.x > nearestEnemy.transform.position.x)
                {
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
                    //unit is to upper left of target, move lower right
                    if (gameObject.transform.position.z > nearestEnemy.transform.position.z)
                    {
                        target.y = target.y - hexGrid.HexSize;
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
            
            if (inRange())
                {
                    fight();
                }

            if (move) 
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, speed);
            }
            //Calling trait bonus
            BoostTraitsWithinRange(); 

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

