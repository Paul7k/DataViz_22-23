using Unity.Mathematics;
using UnityEngine;

public class Vector3Util
{
    public static Vector3 MultiplySeparately(Vector3 vectorOne, Vector3 vectorTwo)
    {
        var vector = Vector3.zero;
        vector.x = math.mul(vectorOne.x, vectorTwo.x);
        vector.y = math.mul(vectorOne.y, vectorTwo.y);
        vector.z = math.mul(vectorOne.z, vectorTwo.z);
        return vector;
    }
    public static Vector3Int MultiplySeparately(Vector3Int vectorOne, Vector3Int vectorTwo)
    {
        var vector = Vector3Int.zero;
        vector.x = math.mul(vectorOne.x, vectorTwo.x);
        vector.y = math.mul(vectorOne.y, vectorTwo.y);
        vector.z = math.mul(vectorOne.z, vectorTwo.z);
        return vector;
    }
}