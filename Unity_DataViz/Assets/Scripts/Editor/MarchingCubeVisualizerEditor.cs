using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingCubeVisualizer))] 
public class MarchingCubeVisualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var source = (MarchingCubeVisualizer) target;
        base.OnInspectorGUI();
        
        GUILayout.Label("Points");
        if(GUILayout.Button("Visualize Sampled Points"))
            source.StartVisualizeSampledPoints();
        if(GUILayout.Button("Remove Sampled Points"))
            source.ResetPoints();
        if(GUILayout.Button("Show only plane"))
            source.ShowPointPlane();
        
        GUILayout.Label("Cubes");
        if(GUILayout.Button("Visualize Sampled Cubes"))
            source.StartVisualizeSampledCubes();
        if(GUILayout.Button("Remove Sampled Cubes"))
            source.ResetCubes();
        if(GUILayout.Button("Show only plane"))
            source.ShowCubePlane();
    }
}
