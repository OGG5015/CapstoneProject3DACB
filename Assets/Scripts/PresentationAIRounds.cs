using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationAIRounds : MonoBehaviour
{
    public HexGrid hexGrid; 
    public GameObject[] unitPrefabs; 
    public int unitsToSpawn = 5; 

    public void SpawnUnits()
    {
        int halfHeight = hexGrid.Height / 2; 

        for (int i = 0; i < unitsToSpawn; i++)
        {
            int randomX = Random.Range(0, hexGrid.Width);
            int randomZ = Random.Range(0, halfHeight);

            Vector3 position = hexGrid.transform.position + HexMetrics.Center(hexGrid.HexSize, randomX, randomZ, hexGrid.Orientation);

            GameObject unitPrefab = unitPrefabs[Random.Range(0, unitPrefabs.Length)];

            Instantiate(unitPrefab, position, Quaternion.identity);
        }
    }

    public void SpawnUnitsEachRound()
    {
        // Need to change this code so that each planning phase, the units
        // are spawned on the top half of the hex grid
        // Note: call SpawnUnits method here or invoke it using 
        //InvokeRepeating for periodic spawning?
    }
}
