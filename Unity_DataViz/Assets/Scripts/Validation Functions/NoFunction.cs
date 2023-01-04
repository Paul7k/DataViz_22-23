using UnityEngine;

public class NoFunction : Validatable
{
    public override float Validate(Vector3 samplePoint)
    {
        return 0f;
    }
}