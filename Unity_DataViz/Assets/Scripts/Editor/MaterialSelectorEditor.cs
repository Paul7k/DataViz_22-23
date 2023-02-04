using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialSelector))]
public class MaterialSelectorEditor : Editor
{
    
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var source = (MaterialSelector) target;
        var matUsages = source.matUsages;

        for (var i = 0; i < matUsages.Count; i++)
        {
            if(GUILayout.Button(matUsages[i].usage))
                source.ApplyMat(i);
        }
    }
}
