using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MarchingCube))]
public class MarchingCubeVisualizer : MonoBehaviour
{
    private MarchingCube source;

    private void Awake()
    {
        source = GetComponent<MarchingCube>();
    }
    
    public GameObject prefab;
    public bool delayed = true;
    public float delay = 0.1f;

    #region Points

    public GameObject[] points;
    public Transform pointsParentObj;

    public Gradient pointsGradient = new Gradient();
    public float pointsGradientScaling = 5f;
    
    public Plane pointsPlane;
    public int[] pointsPlaneIndices;
    [Range(0,0.2f)]
    public float pointsOtherPlaneOpacity = 0.01f;
    
    #endregion
    
    #region Cubes

    public GameObject[] cubes;
    public Transform cubesParentObj;

    public Gradient cubesGradient = new Gradient();
    public float cubesGradientScaling = 5f;
    
    public Plane cubesPlane;
    public int[] cubesPlaneIndices;
    [Range(0,0.2f)]
    public float cubesOtherPlaneOpacity = 0.01f;
    
    #endregion



    public void StartVisualizeSampledPoints()
    {
        var length = source.samplePoints.Length;
        var values = source.samplePoints;

        if (delayed)
            StartCoroutine(CO_VisualizeSampledPoints(length, values, pointsParentObj, Vector3.zero, pointsGradient, pointsGradientScaling));
        else
            points = Visualize(length, values, pointsParentObj, Vector3.zero, pointsGradient, pointsGradientScaling);
    }
    private IEnumerator CO_VisualizeSampledPoints(int length, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, values[i], parentObj, posOffset, gradient, gradientScaling);
            yield return new WaitForSeconds(delay);
        }

        points = objs;
    }

    public void ResetPoints()
    {
        points = DeleteObjects(points);
    }

    public void ShowPointPlane()
    {
        HighlightPlanes(pointsPlane, pointsPlaneIndices, pointsOtherPlaneOpacity, points);
    }
    
    public void StartVisualizeSampledCubes()
    {
        var length = source.sampleCubes.Length;
        var values = Cube.GetConfigsFloat(source.sampleCubes);

        if (delayed)
            StartCoroutine(CO_VisualizeSampledCubes(length, values, cubesParentObj, source.stepSize * 0.5f, cubesGradient, cubesGradientScaling));
        else
            cubes = Visualize(length, values, cubesParentObj, source.stepSize * 0.5f, cubesGradient, cubesGradientScaling);
    }
    private IEnumerator CO_VisualizeSampledCubes(int length, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, values[i], parentObj, posOffset, gradient, gradientScaling);
            yield return new WaitForSeconds(delay);
        }

        cubes = objs;
    }
    public void ResetCubes()
    {
        cubes = DeleteObjects(cubes);
    }
    public void ShowCubePlane()
    {
        HighlightPlanes(cubesPlane, cubesPlaneIndices, cubesOtherPlaneOpacity, cubes);
    }
    
    
    private GameObject[] Visualize(int length, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, values[i], parentObj, posOffset, gradient, gradientScaling);
        }

        return objs;
    }

    private GameObject CreateVizObj(int index, float value, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var pos = DataStruc.GetLocalPosition(index);
        var obj = Instantiate(prefab, parentObj);
        var trans = obj.GetComponent<RectTransform>();
        trans.position = new Vector3(pos.x + posOffset.x, pos.y + posOffset.y, pos.z + posOffset.z);
        var text = obj.GetComponent<TMP_Text>();
        text.text = value.ToString("F");
        text.color = gradient.Evaluate(value / gradientScaling);
        
        return obj;
    }


    #region Plane

    private void HighlightPlanes(Plane plane, int[] planeIndices, float opacity, GameObject[] objs)
    {
        if (plane == Plane.None)
        {
            ResetPlaneHighlight(objs);
            return;
        }
        var planeNormalizedVector = GetPlaneVector();
        var planeVectors = new Vector3Int[planeIndices.Length];
        for (var i = 0; i < planeVectors.Length; i++)
        {
            planeVectors[i] = planeNormalizedVector * planeIndices[i];
        }
        for (var i = 0; i < objs.Length; i++)
        {
            var pos = DataStruc.GetPosition(i);
            var layerPos = Vector3Util.MultiplySeparately(pos, planeNormalizedVector);
            if (planeVectors.Contains(layerPos))
                objs[i].GetComponent<TMP_Text>().alpha = 1;
            else
                objs[i].GetComponent<TMP_Text>().alpha = opacity;
        }
    }

    private void ResetPlaneHighlight(GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            obj.GetComponent<TMP_Text>().alpha = 1;
        }
    }

    public enum Plane
    {
        X, Y, Z, None
    }
    private Vector3Int GetPlaneVector()
    {
        var planeVector = pointsPlane switch
        {
            Plane.X => Vector3Int.right,
            Plane.Y => Vector3Int.up,
            Plane.Z => Vector3Int.forward,
            _ => Vector3Int.zero
        };

        return planeVector;
    }
    #endregion
    

    private GameObject[] DeleteObjects(GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            Destroy(obj);
        }

        return Array.Empty<GameObject>();
    }
    
    
}