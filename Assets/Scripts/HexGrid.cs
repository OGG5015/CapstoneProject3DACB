using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class HexGrid : MonoBehaviour
{

    [field: SerializeField] public HexOrientation Orientation { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public float HexSize { get; private set; }
    [field: SerializeField] public GameObject HexPrefab { get; private set; }

    private void Start()
    {
        GenerateHexGrid();
    }

    private void GenerateHexGrid()
    {
        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 centerPosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;

//                GameObject hex = Instantiate(HexPrefab, centerPosition, Quaternion.identity);
 //               hex.transform.SetParent(transform); 

          //      HexCell hexCell = hex.GetComponent<HexCell>();
                //hexCell.SetupCell(x, z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;

                float percentage = (float)z / Height;

                Color color = new Color(0f, percentage, 1f - percentage);

                Gizmos.color = color;

                for (int s = 0; s < HexMetrics.Corners(HexSize, Orientation).Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + HexMetrics.Corners(HexSize, Orientation)[s % 6],
                        centrePosition + HexMetrics.Corners(HexSize, Orientation)[(s + 1) % 6]
                        );
                }
            }
        }
    }
}




public enum HexOrientation
{
    FlatTop,
    PointyTop
}