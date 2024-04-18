using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBenchMetrics : MonoBehaviour
{
    public static Vector3[] Corners(Vector3 position, int width, float squareSize)
    {
        /*Vector3[] corners = new Vector3[4];

        float halfWidth = (width - 1) * squareSize / 2f;
        float halfSquareSize = squareSize / 2f;

        corners[0] = position - new Vector3(halfWidth, 0, halfSquareSize);
        corners[1] = position + new Vector3(halfWidth, 0, halfSquareSize);
        corners[2] = position - new Vector3(halfWidth, 0, -halfSquareSize);
        corners[3] = position + new Vector3(halfWidth, 0, -halfSquareSize);

        return corners;

        */

        Vector3[] corners = new Vector3[4];

        float halfWidth = (width - 1) * squareSize / 2f;
        float halfSquareSize = squareSize / 2f;

        corners[0] = position + new Vector3(-halfWidth, 0, -halfSquareSize);
        corners[1] = position + new Vector3(halfWidth, 0, -halfSquareSize);
        corners[2] = position + new Vector3(halfWidth, 0, halfSquareSize);
        corners[3] = position + new Vector3(-halfWidth, 0, halfSquareSize);

        return corners;
    }

    public static Vector3 CenterPosition(Vector3 position, int width, float squareSize)
    {
        float halfWidth = (width - 1) * squareSize / 2f;
        return position + new Vector3(0, 0, 0);
    }
}
