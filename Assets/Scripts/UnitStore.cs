using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStore : MonoBehaviour
{
    public GameObject unit1, unit2, unit3;
    public int numOfUnits = 3;

    //[field: SerializeField] public UnitBenchMeshGenerator greg {get; private set;}
    [field: SerializeField] public UnitBench mindy {get; private set;}
    private HexGrid hexgrid;
    //private UnitBench mindy;
    private Vector3 cellCenter;

    void Start()
    {
        FillUnitBench();
    }

    public void FillUnitBench()
    {
        int width = mindy.GetWidth();


        /*for (float i = 0f; i < width; i++)
        {
            Vector3 position = greg.GetCellCenter(i + 1);
            System.Random rnd = new System.Random();
            int dice = rnd.Next(0, numOfUnits);
            switch (dice)
            {
                case 0:
                    Instantiate(unit1, position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(unit2, position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(unit3, position, Quaternion.identity);
                    break;
                default:
                    Debug.Log("Dice for Random Unit in Unit Bench is out of range. Please check numOfUnits or GameObject unit# or case#");
                    break;
            }
            Debug.Log("Spawned unit at: " + position);
        }*/

        int index = 1;
        

            if (mindy != null)
            {

                Vector3 benchOrigin = new Vector3(
                    mindy.transform.position.x - (mindy.Width + (mindy.SquareSize / 2)) * mindy.SquareSize / /*2f*/ mindy.Width,
                    mindy.transform.position.y,
                    mindy.transform.position.z
                );

                float distanceFromOriginX = benchOrigin.x;
                int cellIndex = Mathf.FloorToInt(distanceFromOriginX / mindy.SquareSize);
                int ourCellIndex = cellIndex;

                float cellCenterX = benchOrigin.x + (ourCellIndex + 0.5f) * mindy.SquareSize;
                float cellCenterZ = mindy.transform.position.z;
                cellCenter = new Vector3(cellCenterX, mindy.transform.position.y, cellCenterZ);

                
                Instantiate(unit1, cellCenter, Quaternion.identity);

                Vector3 unit2Cell = new Vector3(cellCenterX + mindy.SquareSize, mindy.transform.position.y, cellCenterZ);
                Instantiate(unit2, unit2Cell, Quaternion.identity);

                Vector3 unit3Cell = new Vector3(cellCenterX + mindy.SquareSize + mindy.SquareSize, mindy.transform.position.y, cellCenterZ);
                Instantiate(unit3, unit3Cell, Quaternion.identity);

            }
            else
            {
                Debug.Log("Unit bench is null");
            }


        }
        
}
