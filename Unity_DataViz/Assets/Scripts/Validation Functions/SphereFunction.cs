using System;
using UnityEngine;

public class SphereFunction : Validatable
{
    public Vector3 center = Vector3.zero;
    public float scale = 1;
    public float offset = 0;
    
    public override float Validate(Vector3 samplePoint)
    {
        return (Vector3.Distance(center, samplePoint) + offset) * scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center,0.05f);
    }
}