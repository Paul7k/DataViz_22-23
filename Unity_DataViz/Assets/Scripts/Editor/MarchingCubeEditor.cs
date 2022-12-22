using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MarchingCube))]
public class MarchingCubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MarchingCube source = (MarchingCube) target;
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Sample Points"))
            source.SamplePoints();
        if(GUILayout.Button("Sample Cube Indices"))
            source.SampleCubesIndices();
        
    }
}