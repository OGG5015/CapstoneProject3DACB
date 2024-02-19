using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class UnitBenchMeshGenerator : MonoBehaviour
{
    [field: SerializeField] public LayerMask gridLayer { get; private set; }
    [field: SerializeField] public UnitBench unitBench { get; private set; }
    public Transform explosionTest;

    private void Awake()
    {
        unitBench = GetComponentInParent<UnitBench>();
        Debug.Log("Unit bench: " + unitBench);

        if (unitBench == null)
        {
            Debug.LogError("UnitBenchMeshGenerator could not find a UnitBench component in its parent or itself");
        }
    }

    private void OnEnable()
    {
        MouseController.instance.OnLeftMouseClick += OnLeftMouseClick;
        //MouseController.instance.OnRightMouseClick += OnRightMouseClick;
    }

    private void OnDisable()
    {
        MouseController.instance.OnLeftMouseClick -= OnLeftMouseClick;
        //MouseController.instance.OnRightMouseClick -= OnRightMouseClick;
    }

    public void CreateUnitBenchMesh()
    {
        CreateUnitBenchMesh(unitBench.Width, unitBench.SquareSize, gridLayer);
    }

    public void CreateUnitBenchMesh(UnitBench unitBench, LayerMask layerMask)
    {
        this.unitBench = unitBench;
        this.gridLayer = layerMask;
        CreateUnitBenchMesh(unitBench.Width, unitBench.SquareSize, gridLayer);
    }

    public void CreateUnitBenchMesh(int Width, float SquareSize, LayerMask layerMask)
    {
        ClearUnitBenchMesh();
        Vector3[] vertices = new Vector3[4 * (Width + 1)];

        float halfWidth = (Width - 1) * SquareSize / (2f);
        float halfSquareSize = SquareSize / 2f;

        for (int i = 0; i <= Width; i++)
        {
            float xPos = unitBench.transform.position.x + i * SquareSize - halfWidth + halfSquareSize;
            float yPos = unitBench.transform.position.y;
            float zPos = unitBench.transform.position.z;

            vertices[i * 4] = new Vector3(xPos, yPos, zPos);
            vertices[i * 4 + 1] = new Vector3(xPos + SquareSize, yPos, zPos);
            vertices[i * 4 + 2] = new Vector3(xPos + SquareSize, yPos, zPos + SquareSize);
            vertices[i * 4 + 3] = new Vector3(xPos, yPos, zPos + SquareSize);
        }


        int[] triangles = new int[6 * Width];

        for (int i = 0; i < Width; i++)
        {
            triangles[i * 6] = i * 4;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4 + 3;
        }

        Mesh UBmesh = new Mesh();
        UBmesh.name = "Unit Bench Mesh";
        UBmesh.vertices = vertices;
        UBmesh.triangles = triangles;
        UBmesh.RecalculateNormals();
        UBmesh.RecalculateBounds();
        UBmesh.Optimize();

        GetComponent<MeshFilter>().sharedMesh = UBmesh;
        GetComponent<MeshCollider>().sharedMesh = UBmesh;

        int gridLayerIndex = GetLayerIndex(layerMask);
        Debug.Log("Layer Index: " + gridLayerIndex);

        gameObject.layer = gridLayerIndex;
    }

    public void ClearUnitBenchMesh()
    {
        if (GetComponent<MeshFilter>().sharedMesh == null)
            return;
        GetComponent<MeshFilter>().sharedMesh.Clear();
        GetComponent<MeshCollider>().sharedMesh.Clear();
    }

    private int GetLayerIndex(LayerMask layerMask)
    {
        int layerMaskValue = layerMask.value;
        Debug.Log("Layer Mask Value: " + layerMaskValue);

        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & layerMask) != 0)
            {
                Debug.Log("Layer Index Loop: " + i);
                return i;
            }
        }

        return 0;
    }

    private void OnLeftMouseClick(RaycastHit hit)
    {
        Vector3 benchOrigin = new Vector3(
            unitBench.transform.position.x - (unitBench.Width /* - 1 */) * unitBench.SquareSize / 2f,
            unitBench.transform.position.y,
            unitBench.transform.position.z
        );

        Vector3 localPoint = transform.InverseTransformPoint(hit.point);
        float distanceFromOriginX = localPoint.x - benchOrigin.x;
        int cellIndex = Mathf.FloorToInt(distanceFromOriginX / unitBench.SquareSize);
        Debug.Log("Clicked on unit bench cell " + cellIndex);

        float cellCenterX = benchOrigin.x + (cellIndex + /*0.5f*/ 1f) * unitBench.SquareSize;
        float cellCenterZ = unitBench.transform.position.z;
        Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);
        Debug.Log("Center of the clicked cell: " + cellCenter);
    }

}