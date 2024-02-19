using UnityEngine;

public class HexGridAndUnitBench : MonoBehaviour
{
    [Header("Hex Grid Settings")]
    [SerializeField] private HexOrientation hexOrientation;
    [SerializeField] private int hexWidth;
    [SerializeField] private int hexHeight;
    [SerializeField] private float hexSize;
    [SerializeField] private GameObject hexPrefab;

    [Header("Unit Bench Settings")]
    [SerializeField] private int benchWidth;
    [SerializeField] private float benchSquareSize;

    private void Start()
    {
        GenerateHexGrid();
        GenerateUnitBench();
    }

    private void GenerateHexGrid()
    {
        for (int z = 0; z < hexHeight; z++)
        {
            for (int x = 0; x < hexWidth; x++)
            {
                Vector3 centerPosition = HexMetrics.Center(hexSize, x, z, hexOrientation) + transform.position;
                Instantiate(hexPrefab, centerPosition, Quaternion.identity, transform);
            }
        }

    }

    private void DrawHexGrid()
    {
        for (int z = 0; z < hexHeight; z++)
        {
            for (int x = 0; x < hexWidth; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, z, hexOrientation) + transform.position;

                float percentage = (float)z / hexHeight;

                Color color = new Color(0f, percentage, 1f - percentage);

                Gizmos.color = color;

                for (int s = 0; s < HexMetrics.Corners(hexSize, hexOrientation).Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + HexMetrics.Corners(hexSize, hexOrientation)[s % 6],
                        centrePosition + HexMetrics.Corners(hexSize, hexOrientation)[(s + 1) % 6]
                        );
                }
            }
        }
    }

    private void GenerateUnitBench()
    {
        for (int i = 0; i < benchWidth; i++)
        {
            Vector3 benchPosition = transform.position + Vector3.right * i * benchSquareSize;
            DrawSquare(benchPosition, benchSquareSize);
        }
    }

    private void DrawUnitBench()
    {
        Gizmos.color = Color.black;

        for (int i = 0; i < benchWidth; i++)
        {
            Vector3 squarePosition = transform.position + Vector3.right * i * benchSquareSize;
            DrawSquare(squarePosition, benchSquareSize);
        }
    }

    private void DrawSquare(Vector3 position, float size)
    {
        float halfSize = size / 2;

        Vector3 topLeft = position + new Vector3(-halfSize, 0, halfSize);
        Vector3 topRight = position + new Vector3(halfSize, 0, halfSize);
        Vector3 bottomLeft = position + new Vector3(-halfSize, 0, -halfSize);
        Vector3 bottomRight = position + new Vector3(halfSize, 0, -halfSize);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    private void OnDrawGizmos()
    {
        DrawHexGrid();
        DrawUnitBench();
    }
}