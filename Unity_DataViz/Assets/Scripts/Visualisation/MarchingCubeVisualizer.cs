using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class MarchingCubeVisualizer : MonoBehaviour
{
    public MarchingCube source;

    private void Awake()
    {
        if(TryGetComponent(out MarchingCube comp))
            source = comp;
    }
    
    public GameObject prefab;
    public bool delayed = true;
    public float delay = 0.1f;

    #region Points Parameters

    public GameObject[] points;
    public Transform pointsParentObj;

    public Gradient pointsGradient = new Gradient();
    public float pointsGradientScaling = 5f;
    
    public Plane pointsPlane;
    public int[] pointsPlaneIndices;
    [Range(0,0.2f)]
    public float pointsOtherPlaneOpacity = 0.01f;
    
    #endregion
    #region Cubes Parameters

    public GameObject[] cubes;
    public Transform cubesParentObj;

    public Gradient cubesGradient = new Gradient();
    public float cubesGradientScaling = 5f;
    
    public Plane cubesPlane;
    public int[] cubesPlaneIndices;
    [Range(0,0.2f)]
    public float cubesOtherPlaneOpacity = 0.01f;
    
    #endregion
    
    #region Points Functions

    public void StartVisualizeSampledPoints()
    {
        var values = source.getSamplePoints;
        var length = values.Length;
        
        if (delayed)
            StartCoroutine(CO_VisualizeSampledPoints(length, values, pointsParentObj, Vector3.zero, pointsGradient, pointsGradientScaling));
        else
            points = Visualize(length, source.PointSize, values, pointsParentObj, Vector3.zero, pointsGradient, pointsGradientScaling);
    }
    private IEnumerator CO_VisualizeSampledPoints(int length, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, source.PointSize, values[i], parentObj, posOffset, gradient, gradientScaling);
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
        HighlightPlanes(pointsPlane, pointsPlaneIndices, pointsOtherPlaneOpacity, points, source.PointSize);
    }

    #endregion
    #region Cubes Functions

    public void StartVisualizeSampledCubes()
    {
        var values = Cube.GetConfigsFloat(source.getSampleCubes);
        var length = values.Length;

        if (delayed)
            StartCoroutine(CO_VisualizeSampledCubes(length, values, cubesParentObj, source.stepSize * 0.5f, cubesGradient, cubesGradientScaling));
        else
            cubes = Visualize(length, source.CubeSize, values, cubesParentObj, source.stepSize * 0.5f, cubesGradient, cubesGradientScaling);
    }
    private IEnumerator CO_VisualizeSampledCubes(int length, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, source.CubeSize,values[i], parentObj, posOffset, gradient, gradientScaling);
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
        HighlightPlanes(cubesPlane, cubesPlaneIndices, cubesOtherPlaneOpacity, cubes, source.CubeSize);
    }

    #endregion
    
    private GameObject[] Visualize(int length, Vector3Int size, float[] values, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var objs = new GameObject[length];
        for (var i = 0; i < length; i++)
        {
            objs[i] = CreateVizObj(i, size, values[i], parentObj, posOffset, gradient, gradientScaling);
        }

        return objs;
    }
    private GameObject CreateVizObj(int index, Vector3Int size, float value, Transform parentObj, Vector3 posOffset, Gradient gradient, float gradientScaling)
    {
        var pos = DataStruc.GetLocalPosition(index, size);
        var obj = Instantiate(prefab, parentObj);
        var trans = obj.GetComponent<RectTransform>();
        trans.position = new Vector3(pos.x + posOffset.x, pos.y + posOffset.y, pos.z + posOffset.z);
        var text = obj.GetComponent<TMP_Text>();
        text.text = value.ToString("F");
        text.color = gradient.Evaluate(value / gradientScaling);
        
        return obj;
    }
    private GameObject[] DeleteObjects(GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            Destroy(obj);
        }

        return Array.Empty<GameObject>();
    }
    
    #region Plane

    private void HighlightPlanes(Plane plane, int[] planeIndices, float opacity, GameObject[] objs, Vector3Int size)
    {
        if (plane == Plane.None)
        {
            ResetPlaneHighlight(objs);
            return;
        }
        var planeNormalizedVector = GetPlaneVector(plane);
        var planeVectors = new Vector3Int[planeIndices.Length];
        for (var i = 0; i < planeVectors.Length; i++)
        {
            planeVectors[i] = planeNormalizedVector * planeIndices[i];
        }
        for (var i = 0; i < objs.Length; i++)
        {
            var pos = DataStruc.GetPosition(i, size);
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
    private Vector3Int GetPlaneVector(Plane plane)
    {
        var planeVector = plane switch
        {
            Plane.X => Vector3Int.right,
            Plane.Y => Vector3Int.up,
            Plane.Z => Vector3Int.forward,
            _ => Vector3Int.zero
        };

        return planeVector;
    }
    #endregion
}