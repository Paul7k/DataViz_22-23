using System;
using UnityEngine;

[Serializable]
public class Edge
{
    [SerializeField]
    private int[] vertices;
    public int VertexA => vertices[0];
    public int VertexB => vertices[1];

    public Edge(int vertexA, int vertexB)
    {
        vertices = new[] {vertexA, vertexB};
    }
}