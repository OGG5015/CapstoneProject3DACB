using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBench : MonoBehaviour
{
    
    [SerializeField] private int numberOfSquares = 7;
    [SerializeField] private float squareSize = 1.0f;

    private void OnDrawGizmos()
    {
        // Set the color for Gizmos
        Gizmos.color = Color.white;

        for (int i = 0; i < numberOfSquares; i++)
        {
            // Calculate the position for each square
            Vector3 squarePosition = transform.position + Vector3.right * i * squareSize;

            // Draw the square using Gizmos
            DrawSquare(squarePosition, squareSize);
        }
    }

    private void DrawSquare(Vector3 position, float size)
    {
        float halfSize = size / 2;

        // Define the four corners of the square
        Vector3 topLeft = position + new Vector3(-halfSize, 0, halfSize);
        Vector3 topRight = position + new Vector3(halfSize, 0, halfSize);
        Vector3 bottomLeft = position + new Vector3(-halfSize, 0, -halfSize);
        Vector3 bottomRight = position + new Vector3(halfSize, 0, -halfSize);

        // Draw lines to form the square
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}

