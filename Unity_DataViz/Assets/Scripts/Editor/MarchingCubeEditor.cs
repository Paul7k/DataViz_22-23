using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MarchingCube))]
public class MarchingCubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var source = (MarchingCube) target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Sample Points"))
        {
            if(source.validationFunction == null)
                return;
            source.SamplePoints();
        }
        if(GUILayout.Button("Sample Cube Indices"))
            source.SampleCubesIndices();
        
    }
}