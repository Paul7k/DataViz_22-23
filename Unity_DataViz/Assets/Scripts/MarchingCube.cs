using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MarchingCube : MonoBehaviour
{
    public Vector3 stepSize;
    public Vector3Int stepCount;
    public Vector3Int PointSize => stepCount + Vector3Int.one;
    public Vector3Int CubeSize => stepCount;

    private float[] samplePoints;
    public float[] getSamplePoints => samplePoints;
    public Validatable validationFunction;

    public float isoValue = 2f;
    private Cube[] sampleCubes;
    public Cube[] getSampleCubes => sampleCubes;

    public bool interpolated = true;
    
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    
    public MeshFilter meshFilter;

    private void OnEnable()
    {
        DataStruc.SetMarchingCube(this);
    }

    public void SamplePoints()
    {
        var length = PointSize.x * PointSize.y * PointSize.z;
        samplePoints = new float[length];
        for (var i = 0; i < length; i++)
        {
            var pos = DataStruc.GetLocalPosition(i, PointSize);
            var value = validationFunction.Validate(pos);
            samplePoints[i] = value;
        }
    }

    public void SampleCubesIndices()
    {
        sampleCubes = new Cube[CubeSize.x * CubeSize.y * CubeSize.z];
        for (var i = 0; i < sampleCubes.Length; i++)
        {
            sampleCubes[i] = CreateCubeConfig(i);
        }
    }

    private Cube CreateCubeConfig(int cubeIndex)
    {
        var pointIndices = DataStruc.GetPointIndices(cubeIndex);
        var config = 0;
        for (var i = 0; i < pointIndices.Length; i++)
        {
            var bitshift = (int) Mathf.Pow(2, i);
            if(samplePoints[pointIndices[i]] < isoValue) config |= bitshift;
        }
        return new Cube(config, pointIndices);
    }

    
    public void CreateMeshData()
    {
        vertices.Clear();
        triangles.Clear();
        var triOffset = 0;

        foreach (var cube in sampleCubes)
        {
            
            //Start ForEachLoop
            var edges = LookupTable.Tris[cube.config];
            //DataStruc.DebugList(edges);
            
            var vertIndices = new List<int>();
            foreach (var edge in edges)
            {
                if(edge == -1)
                    break;
                vertIndices.Add(edge);
            }
            //DataStruc.DebugList(vertIndices);

            var verts = new List<Vector3>();
            foreach (var vertIndex in vertIndices)
            {
                var generalIndexA = LookupTable.Edges[vertIndex, 0];
                var generalIndexB = LookupTable.Edges[vertIndex, 1];
                var specificIndexA = cube.pointIndices[generalIndexA];
                var specificIndexB = cube.pointIndices[generalIndexB];
                var vertexPosition = GetMid(specificIndexA, specificIndexB);
                verts.Add(vertexPosition);
            }
            //DataStruc.DebugList(verts);

            var tris = new List<int>();
            // for (var i = 0; i < verts.Count; i++)
            // {
            //     tris.Add(i + triOffset);
            // }
            for (var i = verts.Count-1; i >= 0; i--)
            {
                tris.Add(i + triOffset);
            }
            //DataStruc.DebugList(tris);
        
            vertices.AddRange(verts);
            triangles.AddRange(tris);
            triOffset += tris.Count;
            //Debug.Log(triOffset);
            //End of ForEachLoop
        }
    }

    
    public void CreateMesh()
    {
        var mesh = meshFilter.sharedMesh;
        if(mesh == null)
            mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32; //Larger Arrays
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        mesh.RecalculateNormals();
        
        meshFilter.sharedMesh = mesh;
    }

    public void Calculate()
    {
        SamplePoints();
        SampleCubesIndices();
        CreateMeshData();
        CreateMesh();
    }
    
    private Vector3 GetMidPoint(int indexA, int indexB)
    {
        var positionA = DataStruc.GetLocalPosition(indexA, PointSize);
        var positionB = DataStruc.GetLocalPosition(indexB, PointSize);
        
        return Interpolate(positionA, positionB, 0.5f);
    }
    
    private Vector3 GetMid(int indexA, int indexB)
    {
        if (!interpolated)
            return GetMidPoint(indexA, indexB);
        
        var valueA = samplePoints[indexA];
        var valueB = samplePoints[indexB];
        var positionA = DataStruc.GetLocalPosition(indexA, PointSize);
        var positionB = DataStruc.GetLocalPosition(indexB, PointSize);

        var t = GetTValue(valueA, valueB);
        return Interpolate(positionA, positionB, t);
    }

    private Vector3 Interpolate(Vector3 positionA, Vector3 positionB, float t)
    {
        return positionA + t * (positionB - positionA); 
    }
    private float GetTValue(float valueA, float valueB)
    {
        return (isoValue - valueA) / (valueB - valueA);
    }
    

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.HSVToRGB(.0f, .0f, .36f);

        var basePos = transform.position;
        var size = Vector3Util.MultiplySeparately(stepCount, stepSize);
        
        Gizmos.DrawWireCube(basePos + size * 0.5f, size);
        
        Gizmos.color = Color.HSVToRGB(.04f, .98f, .62f);
        var stepX = Vector3Util.MultiplySeparately(stepSize, Vector3.right);
        for (var x = 1; x < stepCount.x; x++)
        {
            var pos = basePos + stepX * x;
            Gizmos.DrawWireCube(pos + stepSize * 0.5f, stepSize);
        }
        
        Gizmos.color = Color.HSVToRGB(.24f, .98f, .67f);
        var stepY = Vector3Util.MultiplySeparately(stepSize, Vector3.up);
        for (var y = 1; y < stepCount.y; y++)
        {
            var pos = basePos + stepY * y;
            Gizmos.DrawWireCube(pos + stepSize * 0.5f, stepSize);
        }
        
        Gizmos.color = Color.HSVToRGB(.55f, .97f, .75f);
        var stepZ = Vector3Util.MultiplySeparately(stepSize, Vector3.forward);
        for (var z = 1; z < stepCount.z; z++)
        {
            var pos = basePos + stepZ * z;
            Gizmos.DrawWireCube(pos + stepSize * 0.5f, stepSize);
        }
    }

    #endregion
}
