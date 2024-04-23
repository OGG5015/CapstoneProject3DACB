/*using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStore : MonoBehaviour
{
    public GameObject unit1, unit2, unit3;
    public int numOfUnits = 3;
    public BarTimer timer;
    //[field: SerializeField] public UnitBenchMeshGenerator greg {get; private set;}
    [field: SerializeField] public UnitBench mindy {get; private set;}
    private HexGrid hexgrid;
    //private UnitBench mindy;
    private Vector3 cellCenter;
    public bool[] isBenchPosFull;

    void Start()
    {
        timer = GameObject.Find("timerFill").GetComponent<BarTimer>();
        isBenchPosFull = new bool[mindy.GetWidth()];
        for (int i = 0; i < isBenchPosFull.Length; i++)
        {
            isBenchPosFull[i] = false;
        }

        FillUnitBench();
    }

    public void FillUnitBench()
    {
        int width = mindy.GetWidth();

        int index = 1;

        if (mindy != null)
        {

            Vector3 benchOrigin = new Vector3(
                mindy.transform.position.x - (mindy.Width + (mindy.SquareSize / 2)) * mindy.SquareSize / /*2f*//* mindy.Width,
    /*            mindy.transform.position.y,
                mindy.transform.position.z
            );

            float distanceFromOriginX = benchOrigin.x;
            int cellIndex = Mathf.FloorToInt(distanceFromOriginX / mindy.SquareSize);
            int ourCellIndex = cellIndex;

            float cellCenterX = benchOrigin.x + (ourCellIndex + 0.5f) * mindy.SquareSize;
            float cellCenterZ = mindy.transform.position.z;
            cellCenter = new Vector3(cellCenterX, mindy.transform.position.y, cellCenterZ);

            // loop that fill bench
            GameObject unitX = unit1;
            for (int i = 0; i < 7; i++)
            {
                // randomize units that spawn
                System.Random rnd = new System.Random();
                int dice = rnd.Next(0, numOfUnits);
                switch (dice)
                {
                    case 0:
                        unitX = unit1; break;
                    case 1:
                        unitX = unit2; break;
                    case 2:
                        unitX = unit3; break;
                    default:
                        Debug.Log("numOfUnits exceeds number of avalible units (or some other error)"); break;
                }

                if (isBenchPosFull[i] == false)
                {
                    Vector3 unitCell = new Vector3(cellCenterX + (mindy.SquareSize * i), mindy.transform.position.y, cellCenterZ);
                    var u1 = Instantiate(unitX, unitCell, Quaternion.identity);
                    u1.gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
                    u1.transform.parent = GameObject.Find("Grid").transform;
                    isBenchPosFull[i] = true;
                }
            }

            //var u1 =Instantiate(unit1, cellCenter, Quaternion.identity);
            //u1.gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
            //u1.transform.parent = GameObject.Find("Grid").transform;

            //Vector3 unit2Cell = new Vector3(cellCenterX + mindy.SquareSize, mindy.transform.position.y, cellCenterZ);
            //Instantiate(unit2, unit2Cell, Quaternion.identity);

            //Vector3 unit3Cell = new Vector3(cellCenterX + mindy.SquareSize + mindy.SquareSize, mindy.transform.position.y, cellCenterZ);
            //Instantiate(unit3, unit3Cell, Quaternion.identity);

        }
        else
        {
            Debug.Log("Unit bench is null");
        }


    }
        
}*/
