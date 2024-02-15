using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitBenchMeshGenerator))]
public class UnitBenchMeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UnitBenchMeshGenerator benchMeshGenerator = (UnitBenchMeshGenerator)target;

        if (GUILayout.Button("Create Unit Bench Mesh"))
        {
            benchMeshGenerator.CreateUnitBenchMesh();
        }

        if (GUILayout.Button("Clear Unit Bench Mesh"))
        {
            benchMeshGenerator.ClearUnitBenchMesh();
        }
    }
}
