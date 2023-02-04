using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveLoadMesh))]
public class SaveLoadMeshEditor : Editor
{
    private string feedbackText = "";
    
    public override void OnInspectorGUI()
    {
        var source = (SaveLoadMesh) target;
        
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
            ResetFeedbackText();
        
        if (GUILayout.Button("Load"))
            feedbackText = source.Load();

        EditorGUI.BeginChangeCheck();
        source.filename = EditorGUILayout.TextField("Filename", source.filename);
        if (EditorGUI.EndChangeCheck())
            ResetFeedbackText();
        
        if (GUILayout.Button("Save"))
            feedbackText = source.Save();
        
        GUILayout.Space(8f);
        switch (feedbackText)
        {
            case "":
                return;
            case " ":
                EditorGUILayout.HelpBox("The Mesh \'" + source.filename + "\' was saved", MessageType.Info);
                break;
            default:
                EditorGUILayout.HelpBox(feedbackText, MessageType.Warning);
                break;
        }
    }
    
    private void ResetFeedbackText(){ feedbackText = ""; }
}