using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStore : MonoBehaviour
{
    public GameObject unit1, unit2, unit3;
    public int numOfUnits = 3;
    public UnitBenchMeshGenerator greg;
    public UnitBench mindy;

    void Start()
    {
        FillUnitBench();
    }

    public void FillUnitBench()
    {
        int width = mindy.GetWidth();
        

        for (float i = 0f; i < width; i++)
        {
            Vector3 position = greg.GetCellCenter(i);
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
        }
    }
}
