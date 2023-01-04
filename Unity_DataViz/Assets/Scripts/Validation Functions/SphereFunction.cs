using UnityEngine;

public class SphereFunction : Validatable
{
    public Vector3 centerPoint = Vector3.zero;

    public override float Validate(Vector3 samplePoint)
    {
        return Vector3.Distance(centerPoint, samplePoint);
    }
}