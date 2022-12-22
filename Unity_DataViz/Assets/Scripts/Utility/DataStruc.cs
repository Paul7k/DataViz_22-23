using System;
using UnityEngine;

public class DataStruc
{
    private static MarchingCube _marchingCube;

    public static void SetMarchingCube(MarchingCube marchingCube)
    {
        _marchingCube = marchingCube;
    }
    public static int GetIndex(int x, int y, int z)
    {
        var size = _marchingCube.PointSize;
        return x +
               z * size.x +
               y * size.x * size.z;
    }
    public static int GetIndex(Vector3Int vector)
    {
        return GetIndex(vector.x, vector.y, vector.z);
    }
    public static Vector3Int GetPosition(int index)
    {
        var size = _marchingCube.PointSize;
        var y = Math.DivRem(index, size.z * size.x, out var rem);
        var z = Math.DivRem(rem, size.x, out var x);
        return new Vector3Int(x, y, z);
    }
    public static Vector3 GetLocalPosition(Vector3Int position)
    {
        var stepSize = _marchingCube.stepSize;
        return Vector3Util.MultiplySeparately(position, stepSize);
    }
    public static Vector3 GetLocalPosition(int index)
    {
        return GetLocalPosition(GetPosition(index));
    }
    public static Vector3 GetGlobalPosition(Vector3Int position)
    {
        var transform = _marchingCube.transform;
        var local = GetLocalPosition(position);
        return local + transform.position;
    }
    public static Vector3 GetGlobalPosition(int index)
    {
        var transform = _marchingCube.transform;
        var local = GetLocalPosition(GetPosition(index));
        return local + transform.position;
    }

    public static int[] GetPointIndices(int cubeIndex)
    {
        var pointSize = _marchingCube.PointSize;
        var pointIndices = new int[8];
        
        //Lower plane
        pointIndices[0] = cubeIndex + pointSize.x;
        pointIndices[1] = cubeIndex + pointSize.x + 1;
        pointIndices[2] = cubeIndex + 1;
        pointIndices[3] = cubeIndex;
        //Upper plane
        var planeOffset = pointSize.x * pointSize.z;
        pointIndices[4] = pointIndices[0] + planeOffset;
        pointIndices[5] = pointIndices[1] + planeOffset;
        pointIndices[6] = pointIndices[2] + planeOffset;
        pointIndices[7] = pointIndices[3] + planeOffset;

        return pointIndices;
    }
}