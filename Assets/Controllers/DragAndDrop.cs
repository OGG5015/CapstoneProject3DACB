using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private HexGrid hexGrid;
    Vector3 mousePosition;
    bool isDragging = false;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

            //new code
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            newPosition.y = transform.position.y;
            transform.position = newPosition;

        }
    }

    private void OnMouseUp()
    {
        SnapToHexOrUnitBench();

        isDragging = false;
    }

    /*private void OnMouseOver()
    {
        DetermineHoveredCell();
    }*/

    private void DetermineHoveredCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            HexGrid hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {
                Vector2 hexCenter = HexMetrics.CoordinateToAxial(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                Debug.Log("Hovered over hex cell: " + hexCenter);

            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }
    }

    /*private void SnapToHexCenter()
    {
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            HexGrid hexGrid = hit.collider.GetComponentInParent<HexGrid>();
            Debug.Log("HexGrid: " + hexGrid);

            if (hexGrid != null)
            {
                Vector2 hexCenter = HexMetrics.CoordinateToAxial(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)hexCenter.x, (int)hexCenter.y, hexGrid.Orientation);

                transform.position = center;

                Debug.Log("Hex Center: " + hexCenter);
                Debug.Log("Snapped to: " + center);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }
    }*/
    private void SnapToHexCenter()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {
                Vector2 hexCenter = HexMetrics.CoordinateToAxial(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)hexCenter.x, (int)hexCenter.y, hexGrid.Orientation);

                transform.position = center;

                Debug.Log("Hex Center: " + hexCenter);
                Debug.Log("Snapped to: " + center);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }*/

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        /*if (Physics.Raycast(ray, out hit))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {
                Vector2 hexCenter = HexMetrics.CoordinateToAxial(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)hexCenter.x, (int)hexCenter.y, hexGrid.Orientation);

                // Adjust the center position based on the orientation
                if (hexGrid.Orientation == HexOrientation.PointyTop)
                {
                    transform.position = new Vector3(center.x, transform.position.y, center.z);
                }
                else
                {
                    transform.position = center;
                }

                Debug.Log("Hex Center: " + hexCenter);
                Debug.Log("Snapped to: " + transform.position);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }*/

        /*if (Physics.Raycast(ray, out hit))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {
                Vector3 localPosition = hexGrid.transform.InverseTransformPoint(hit.point);

                Vector2 offsetCoordinates = HexMetrics.CoordinateToOffset(localPosition.x, localPosition.z, hexGrid.HexSize, hexGrid.Orientation);

                offsetCoordinates = HexMetrics.AxialRound(offsetCoordinates);

                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)offsetCoordinates.x, (int)offsetCoordinates.y, hexGrid.Orientation);

                transform.position = center;

                Debug.Log("Hex Center: " + offsetCoordinates);
                Debug.Log("Snapped to: " + center);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }*/

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {
                Vector2 offsetCoordinates = HexMetrics.CoordinateToOffset(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                offsetCoordinates = HexMetrics.AxialRound(offsetCoordinates);

                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)offsetCoordinates.x, (int)offsetCoordinates.y, hexGrid.Orientation);

                // Adjust the position based on the orientation
                if (hexGrid.Orientation == HexOrientation.PointyTop)
                {
                    transform.position = new Vector3(center.x, transform.position.y, center.z);
                }
                else
                {
                    transform.position = center;
                }

                Debug.Log("Hex Center: " + offsetCoordinates);
                Debug.Log("Snapped to: " + transform.position);
            }
            else
            {
                Debug.Log("Hex grid is null");
            }
        }
    }
}

