using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class UnitBenchMeshGenerator : MonoBehaviour
{
    [field: SerializeField] public LayerMask gridLayer { get; private set; }
    [field: SerializeField] public UnitBench unitBench { get; private set; }
    public Transform explosionTest;
    public UIStore shop;

    private void Awake()
    {
        unitBench = GetComponentInParent<UnitBench>();
        Debug.Log("Unit bench: " + unitBench);

        if (unitBench == null)
        {
            Debug.LogError("UnitBenchMeshGenerator could not find a UnitBench component in its parent or itself");
        }

    }

    private void Start()
    {
        shop = GameObject.Find("UIStore").GetComponent<UIStore>();
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

        Mesh UBmesh = new Mesh();
        UBmesh.name = "Unit Bench Mesh";

        Vector3[] vertices;

        vertices = new Vector3[(Width + 1) * (Width + 1)];

        for (int x = 0; x <= Width; x++)
        {
            for (int z = 0; z <= Width; z++)
            {
                vertices[x * (Width + 1) + z] = new Vector3((x * SquareSize) - (SquareSize / 2), 0, z - (SquareSize / 2));
            }
        }

        UBmesh.vertices = vertices;

        int[] triangles = new int[Width * Width * 6];

        int triangleIndex = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Width; z++)
            {
                int vertexIndex = x * (Width + 1) + z;

                triangles[triangleIndex++] = vertexIndex;
                triangles[triangleIndex++] = vertexIndex + 1;
                triangles[triangleIndex++] = vertexIndex + (Width + 1);

                triangles[triangleIndex++] = vertexIndex + 1;
                triangles[triangleIndex++] = vertexIndex + (Width + 1) + 1;
                triangles[triangleIndex++] = vertexIndex + (Width + 1);
            }
        }

        UBmesh.triangles = triangles;
        UBmesh.vertices = vertices;

        UBmesh.RecalculateNormals();
        UBmesh.RecalculateBounds();

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
        /*Vector3 benchOrigin = new Vector3(
            unitBench.transform.position.x - (unitBench.Width  /*- 1 ) * unitBench.SquareSize / 2f,
            unitBench.transform.position.y,
            unitBench.transform.position.z
        );*/



        Vector3 benchOrigin = new Vector3(
        unitBench.transform.position.x - (unitBench.Width * unitBench.SquareSize / 2f),
        transform.position.y,
        transform.position.z - (unitBench.Width * unitBench.SquareSize / 2f)
    );


        Vector3 localPoint = transform.InverseTransformPoint(hit.point);
        float distanceFromOriginX = localPoint.x - benchOrigin.x;
        int cellIndex = Mathf.FloorToInt(distanceFromOriginX / unitBench.SquareSize);
        shop.isBenchPosFull[cellIndex] = false; // notify store
        Debug.Log("Bench is " + shop.isBenchPosFull[cellIndex] + " at " + cellIndex);
        Debug.Log("Clicked on unit bench cell " + cellIndex);

        float cellCenterX = benchOrigin.x + (cellIndex + /*0.5f*/ 1f) * unitBench.SquareSize;
        float cellCenterZ = unitBench.transform.position.z;
        Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);
        Debug.Log("Center of the clicked cell: " + cellCenter);

    }

    // return cellCenter if called
    public Vector3 GetCellCenter(float index)
    {
        Vector3 benchOrigin = new Vector3(
        unitBench.transform.position.x - (unitBench.Width * unitBench.SquareSize / 2f),
        transform.position.y,
        transform.position.z - (unitBench.Width * unitBench.SquareSize / 2f));

        if (index == 0) { return benchOrigin; }

        float cellCenterX = benchOrigin.x + (index + 1f) * unitBench.SquareSize;
        float cellCenterZ = unitBench.transform.position.z;
        Vector3 cellCenter = new Vector3(cellCenterX, unitBench.transform.position.y, cellCenterZ);
        Debug.Log("Getting cell center of index " + index + ": " + cellCenter);

        return cellCenter;

    }

    
}

