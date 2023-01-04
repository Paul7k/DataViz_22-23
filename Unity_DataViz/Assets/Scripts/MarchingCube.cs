using UnityEngine;

public class MarchingCube : MonoBehaviour
{
    public Vector3 stepSize;
    public Vector3Int stepCount;
    public Vector3Int PointSize => stepCount + Vector3Int.one;
    public Vector3Int CubeSize => stepCount;

    public float[] samplePoints;
    public Validatable validationFunction;

    public float isoValue = 2f;
    public Cube[] sampleCubes;

    private void Awake()
    {
        DataStruc.SetMarchingCube(this);
    }

    public void SamplePoints()
    {
        var length = PointSize.x * PointSize.y * PointSize.z;
        samplePoints = new float[length];
        for (var i = 0; i < length; i++)
        {
            var pos = DataStruc.GetLocalPosition(i);
            var value = validationFunction.Validate(pos);
            samplePoints[i] = value;
        }
    }

    public void SampleCubesIndices()
    {
        sampleCubes = new Cube[CubeSize.x * CubeSize.y * CubeSize.z];
        for (var cubeIndex = 0; cubeIndex < sampleCubes.Length; cubeIndex++)
        {
            var pointIndices = DataStruc.GetPointIndices(cubeIndex);
            var config = 0;
            for (var i = 0; i < pointIndices.Length; i++)
            {
                var bitshift = (int) Mathf.Pow(2, i);
                if(samplePoints[pointIndices[i]] < isoValue) config |= bitshift;
            }
            sampleCubes[cubeIndex] = new Cube(config, pointIndices);
        }
    }
    

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        var basePos = transform.position;
        var offset = Vector3Util.MultiplySeparately(stepCount, stepSize);
        var endPos = basePos + offset;
        Gizmos.DrawWireCube(basePos + stepSize * 0.5f, stepSize);
        Gizmos.DrawWireCube(endPos - stepSize * 0.5f, stepSize);
        
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
