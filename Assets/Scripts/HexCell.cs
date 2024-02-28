using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell: MonoBehaviour
{
    public void SetupCell(int x, int z)
    {
        
        float rotationAngle = 90f;
        //transform.Rotate(Vector3.up, rotationAngle);
        transform.Rotate(Vector2.up, rotationAngle);
    }

    public void SetupCell(int x, int z, HexOrientation orientation)
    {

        Quaternion rotation = Quaternion.identity;
        switch (orientation)
        {
            case HexOrientation.FlatTop:
                rotation = Quaternion.Euler(30f, 0f, 0f);
                break;
            case HexOrientation.PointyTop:
                rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
        }

        transform.rotation = rotation;
    }

    public void SetupCell(int x, int z, HexOrientation orientation, float hexSize)
    {
        Quaternion rotation = Quaternion.identity;
        switch (orientation)
        {
            case HexOrientation.FlatTop:
                rotation = Quaternion.Euler(30f, 0f, 0f);
                break;
            case HexOrientation.PointyTop:
                rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
        }

        transform.rotation = rotation;

        //float size = hexSize/30;

        //transform.lossyScale = new Vector3(size, size, size);
    }
}
