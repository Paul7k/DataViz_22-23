using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))][ExecuteInEditMode]
public class MaterialSelector : MonoBehaviour
{
    private MeshRenderer renderer;
    public List<MaterialUsage> matUsages = new List<MaterialUsage>();

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void ApplyMat(int index)
    {
        renderer.material = matUsages[index].material;
    }
    
    [Serializable]
    public class MaterialUsage
    {
        public string usage;
        public Material material;
    }
}


