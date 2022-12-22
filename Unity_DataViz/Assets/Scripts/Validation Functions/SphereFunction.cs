using UnityEngine;

public class SphereFunction : IValidatable
{
    public Vector3 CenterPoint = new Vector3(2, 2, 2);
    
    public float Validate(Vector3 samplePoint)
    {
        return Vector3.Distance(CenterPoint, samplePoint);
    }
}