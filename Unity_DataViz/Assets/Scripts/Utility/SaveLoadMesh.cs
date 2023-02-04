using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SaveLoadMesh : MonoBehaviour
{
    public Mesh meshToLoad;
    [HideInInspector]
    public string filename = "Mesh";
    private MeshFilter filter;
    private void Awake()
    {
        SetFilter();
    }

    private void SetFilter()
    {
        if(filter == null)
            filter = GetComponent<MeshFilter>();
    }

    public string Load()
    {
        SetFilter();
        if (meshToLoad == null)
            return "No mesh to load assigned";
        var mesh = Instantiate(meshToLoad);
        filter.sharedMesh = mesh;
        return " ";
    }

    public string Save()
    {
        SetFilter();
        if (filter.sharedMesh == null)
            return "No mesh found";
        
        filename = filename.Replace(' ', '_');
        if (filename == "")
            return "No filename";
        
        var path = "Assets/Meshes/" + filename + ".asset";
        if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path)))
            return "File already exists";

        var mesh = Instantiate(filter.sharedMesh);
        AssetDatabase.CreateAsset(mesh,path);
        AssetDatabase.SaveAssets();
        return " ";
    }
}
