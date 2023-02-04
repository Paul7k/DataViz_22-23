using System;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSpheresFunction : Validatable
{
    public List<Sphere> spheres = new ();

    public override float Validate(Vector3 samplePoint)
    {
        var result = float.MaxValue;
        foreach (var sphere in spheres)
        {
            var value = sphere.Validate(samplePoint);
            if (value < result)
                result = value;
        }

        return result;
    }
    
    [Serializable]
    public class Sphere
    {
        public Vector3 center;
        public float scale;
        public float offset;
        
        public Sphere()
        {
            center = Vector3.zero;
            scale = 1;
            offset = 0;
        }
        
        public float Validate(Vector3 samplePoint)
        {
            return (Vector3.Distance(center, samplePoint) + offset) * scale;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (var sphere in spheres)
        {
            Gizmos.DrawSphere(sphere.center,0.05f);
        }
    }
}