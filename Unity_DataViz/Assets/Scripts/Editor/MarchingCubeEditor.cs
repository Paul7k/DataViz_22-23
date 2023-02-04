using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MarchingCube))]
public class MarchingCubeEditor : Editor
{
    private float fillAmount = 0f;
    
    public override void OnInspectorGUI()
    {
        var source = (MarchingCube) target;
        base.OnInspectorGUI();

        /*
        if (GUILayout.Button("Sample Points"))
        {
            if(source.validationFunction == null)
                return;
            source.SamplePoints();
        }
        if(GUILayout.Button("Sample Cube Indices"))
            source.SampleCubesIndices();
        if(GUILayout.Button("Create Mesh Data"))
            source.CreateMeshData();
        if(GUILayout.Button("Write Mesh"))
            source.CreateMesh();
        */
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if(GUILayout.Button("Calculate"))
            source.Calculate();
        EditorGUI.EndDisabledGroup();
        
    }
}