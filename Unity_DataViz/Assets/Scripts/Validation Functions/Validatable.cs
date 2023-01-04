using UnityEngine;

public abstract class Validatable : MonoBehaviour
{
    public abstract float Validate(Vector3 samplePoint);
}