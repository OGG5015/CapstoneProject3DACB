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
    [field: SerializeField] public GameObject HexPrefabT1 { get; private set; }
    [field: SerializeField] public GameObject HexPrefabT2 { get; private set; }

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
                /*Vector3 centerPosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                GameObject hex = Instantiate(HexPrefabT1, centerPosition, Quaternion.identity);
                hex.transform.SetParent(transform);

                hex.transform.localScale = Vector3.one * (HexSize/2f);

                //AdjustHexSize(hex);

                HexCell hexCell = hex.GetComponent<HexCell>();
                hexCell.SetupCell(x, z);
                //hexCell.SetupCell(x, z, Orientation, HexSize);*/


                Vector3 centerPosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                GameObject hex;

                if (z < Height / 2) // Top half
                {
                    
                    hex = Instantiate(HexPrefabT1, centerPosition, Quaternion.identity);
                }
                else // Bottom half
                {
                    hex = Instantiate(HexPrefabT2, centerPosition, Quaternion.identity);
                }

                hex.transform.SetParent(transform);
                hex.transform.localScale = Vector3.one * (HexSize / 2f);

                HexCell hexCell = hex.GetComponent<HexCell>();
                hexCell.SetupCell(x, z);
            }
        }
    }

    private void AdjustHexSize(GameObject hex)
    {
        float scaleFactor = HexSize / HexMetrics.OuterRadius(1f);

        hex.transform.localScale = new Vector3(0, 0, scaleFactor);
    }

    private void OnDrawGizmos()
    {
        for(int z = 0; z < Height; z ++)
        {
            for(int x = 0; x < Width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;

                float percentage = (float)z / Height;

                Color color = new Color(0f, percentage, 1f - percentage);

                Gizmos.color = color;

                for(int s = 0; s < HexMetrics.Corners(HexSize, Orientation).Length; s++)
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