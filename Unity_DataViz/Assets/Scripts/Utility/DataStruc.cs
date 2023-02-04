using System;
using System.Collections.Generic;
using UnityEngine;

public class DataStruc
{
    private static MarchingCube _marchingCube;

    public static void SetMarchingCube(MarchingCube marchingCube)
    {
        _marchingCube = marchingCube;
    }

    #region --> Index

    /**
    public static int GetIndex(int x, int y, int z)
    {
        var size = _marchingCube.PointSize;
        return x +
               z * size.x +
               y * size.x * size.z;
    }
    **/
    public static int GetIndex(int x, int y, int z, Vector3Int size)
    {
        return x +
               z * size.x +
               y * size.x * size.z;
    }
    /**
    public static int GetIndex(Vector3Int vector)
    {
        return GetIndex(vector.x, vector.y, vector.z);
    }
    **/
    public static int GetIndex(Vector3Int vector, Vector3Int size)
    {
        return GetIndex(vector.x, vector.y, vector.z, size);
    }

    #endregion

    #region --> Position

    /**
    public static Vector3Int GetPosition(int index)
    {
        var size = _marchingCube.PointSize;
        var y = Math.DivRem(index, size.z * size.x, out var rem);
        var z = Math.DivRem(rem, size.x, out var x);
        return new Vector3Int(x, y, z);
    }
    **/
    public static Vector3Int GetPosition(int index, Vector3Int size)
    {
        var y = Math.DivRem(index, size.z * size.x, out var rem);
        var z = Math.DivRem(rem, size.x, out var x);
        return new Vector3Int(x, y, z);
    }
    
    public static Vector3 GetLocalPosition(Vector3Int position)
    {
        var stepSize = _marchingCube.stepSize;
        return Vector3Util.MultiplySeparately(position, stepSize);
    }
    
    /**
    public static Vector3 GetLocalPosition(int index)
    {
        return GetLocalPosition(GetPosition(index));
    }
    **/
    public static Vector3 GetLocalPosition(int index, Vector3Int size)
    {
        return GetLocalPosition(GetPosition(index, size));
    }
    
    public static Vector3 GetGlobalPosition(Vector3Int position)
    {
        var transform = _marchingCube.transform;
        var local = GetLocalPosition(position);
        return local + transform.position;
    }
    public static Vector3 GetGlobalPosition(int index, Vector3Int size)
    {
        var transform = _marchingCube.transform;
        var local = GetLocalPosition(GetPosition(index, size));
        return local + transform.position;
    }

    #endregion
    

    public static int[] GetPointIndices(int cubeIndex)
    {
        var size = _marchingCube.PointSize;
        var pointIndices = new int[8];
        var pivot = CubeToPointIndex(cubeIndex);
        
        //Lower plane
        pointIndices[0] = pivot + size.x;
        pointIndices[1] = pivot + size.x + 1;
        pointIndices[2] = pivot + 1;
        pointIndices[3] = pivot;
        //Upper plane
        var planeOffset = size.x * size.z;
        pointIndices[4] = pointIndices[0] + planeOffset;
        pointIndices[5] = pointIndices[1] + planeOffset;
        pointIndices[6] = pointIndices[2] + planeOffset;
        pointIndices[7] = pointIndices[3] + planeOffset;

        return pointIndices;
    }

    public static int CubeToPointIndex(int cubeIndex)
    {
        var cubePos = GetPosition(cubeIndex, _marchingCube.CubeSize);
        return GetIndex(cubePos, _marchingCube.PointSize);
    }
    public static void DebugList(List<int> list)
    {
        string str = "";
        foreach (var obj in list)
        {
            str += obj + " ";
        }
        Debug.Log(str);
    }
    public static void DebugList(int[] list)
    {
        string str = "";
        foreach (var obj in list)
        {
            str += obj + " ";
        }
        Debug.Log(str);
    }
    public static void DebugList(List<Vector3> list)
    {
        string str = "";
        foreach (var obj in list)
        {
            str += obj + " ";
        }
        Debug.Log(str);
    }
}