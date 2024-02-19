using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBench : MonoBehaviour
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public float SquareSize { get; private set; }

    private void OnDrawGizmos()
    {
        // Set the color for Gizmos
        Gizmos.color = Color.black;
        
        for (int i = 0; i < Width; i++)
        {
            Vector3 squarePosition = transform.position + Vector3.right * i * SquareSize;
            DrawSquare(squarePosition, SquareSize);
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
}