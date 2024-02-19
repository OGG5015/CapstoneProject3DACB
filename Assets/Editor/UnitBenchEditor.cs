/*using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitBench))]
public class UnitBenchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UnitBench unitBench = (UnitBench)target;

        if (GUILayout.Button("Generate Unit Bench Mesh"))
        {
            unitBench.GenerateMesh();
        }

        if (GUILayout.Button("Clear Unit Bench Mesh"))
        {
            unitBench.ClearMesh();
        }
    }
}*/