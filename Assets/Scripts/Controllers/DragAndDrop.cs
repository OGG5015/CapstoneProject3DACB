using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DragAndDrop : MonoBehaviour
{
    private HexGrid hexGrid;
    private UnitBench unitBench;
    Vector3 mousePosition;
    bool isDragging = false;
    Vector3 PrevPos;
    public SFXPlaying dj;

    // NEW CODE
    
    // END NEW CODE
    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        isDragging = true;
        dj.PlayPickup();
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        SnapToHexOrUnitBench();

        isDragging = false;
        dj.PlayDrop();

        GetComponent<PlayerNetwork>().unitPosition.Value = transform.position;
    }

    private void SnapToHexOrUnitBench()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            hexGrid = hit.collider.GetComponent<HexGrid>();
            unitBench = hit.collider.GetComponent<UnitBench>();

            if (hexGrid != null)
            {
                SnapToHexCenter();
                return;
            }
            else if (unitBench != null)
            {
                SnapToUnitBench();
                return;
            }
        }

        SnapToPrevious();

        Debug.Log("Neither hex grid nor unit bench found");

    }


    private void SnapToHexCenter()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hexGrid = hit.collider.GetComponentInParent<HexGrid>();

            if (hexGrid != null)
            {

                Vector2 offsetCoordinates = HexMetrics.CoordinateToOffset(hit.point.x, hit.point.z, hexGrid.HexSize, hexGrid.Orientation);
                offsetCoordinates = HexMetrics.AxialRound(offsetCoordinates);

                Vector3 center = HexMetrics.Center(hexGrid.HexSize, (int)offsetCoordinates.x, (int)offsetCoordinates.y, hexGrid.Orientation);

                if (hexGrid.Orientation == HexOrientation.PointyTop)
                {
                    transform.position = new Vector3(center.x, transform.position.y, center.z);
                    PrevPos = transform.position;
                }
                else
                {
                    transform.position = center;
                    PrevPos = transform.position;
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

    private void SnapToUnitBench()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            unitBench = hit.collider.GetComponentInParent<HexGrid>().GetComponentInChildren<UnitBench>();

            if (unitBench != null)
            {

                Vector3 benchOrigin = new Vector3(
                    unitBench.transform.position.x - (unitBench.Width) * unitBench.SquareSize / 2f,
                    unitBench.transform.position.y,
                    unitBench.transform.position.z
                );

                Vector3 localPoint = hit.point;
                float distanceFromOriginX = localPoint.x - benchOrigin.x;
                int cellIndex = Mathf.FloorToInt(distanceFromOriginX / unitBench.SquareSize);

                float cellCenterX = benchOrigin.x + (cellIndex + 0.5f) * unitBench.SquareSize;
                float cellCenterZ = unitBench.transform.position.z;
                Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);

                transform.position = cellCenter;
                Debug.Log("Snapped to: " + cellCenter);

                PrevPos = transform.position;
            }
            else
            {
                Debug.Log("Unit bench is null");
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing");
        }
        
    }

    private void SnapToPrevious()
    {
        transform.position = PrevPos;
    }

    

}





/*private void OnMouseOver()
    {
        DetermineHoveredCell();
    }

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
    }*/
