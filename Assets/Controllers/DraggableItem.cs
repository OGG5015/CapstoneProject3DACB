using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        if(isDragging)
        {
            HandleDragging();
        }
    }
    void HandleDragging()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        if(Input.GetMouseButton(0))
        {
            isDragging = false;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
