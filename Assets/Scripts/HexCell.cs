using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell: MonoBehaviour
{
    [Header("Cell Properties")]
    [SerializeField] private HexOrientation orientation;
    [field: SerializeField] public HexGrid Grid { get; set; }
    [field: SerializeField] public float HexSize { get; set; }
    //[field:SerializeField] public TerrainType TerrainType { get; set; }
    [field: SerializeField] public Vector2 OffsetCoordinates { get; set; }
    [field: SerializeField] public Vector3 CubeCoordinates { get; set; }
    [field: SerializeField] public Vector2 AxialCoordinates { get; set; }
    [field: NonSerialized] public List<HexCell> Neighbors { get; private set; }

    [field: SerializeField] private Transform terrain { get; /*private*/ set; }
}
