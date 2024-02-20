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
    public bool toggle; //toggles if combat is active or not
    private ArrayList unitList = new ArrayList();
    private bool requireTarget;
    private int target;
    private Vector3 closestTarget;
    private int distance;
    public GameObject self;
    public GameObject nearestEnemy;
    public float speed;

    private HexGrid hexGrid;
    private UnitBench unitBench;
    Vector3 mousePosition;
    bool isDragging = false;

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

    //snap stuff
    

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        }
    }

    private void OnMouseUp()
    {
        SnapToHexOrUnitBench();

        isDragging = false;
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


    private void SnapToHexCenter()
    {

        Ray ray = Camera.main.ScreenPointToRay(self.transform.position);
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
        SnapToHexCenter();
        if (toggle){
            self.transform.position = Vector3.MoveTowards(self.transform.position, nearestEnemy.transform.position, speed);
            SnapToHexCenter();
            //AIFunctionality();

            /*for (int unit = 0; unit < unitList.Count; unit++){
                if (Scan(unit)){
                    Attack(unit, getNearest(unit));
                }
                else {
                    Move(unit, getNearest(unit));
                }
            }*/
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

