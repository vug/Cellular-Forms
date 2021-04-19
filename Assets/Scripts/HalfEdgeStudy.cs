using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdge
{
    public HalfEdge next;
    public HalfEdge twin;
    public Vertex vertex;
    public Edge edge;
    public Face face;
}

public class Vertex
{
    public HalfEdge halfEdge;
    public Vector3 position;

}

public class Edge
{
    public HalfEdge halfEdge;
}

public class Face
{
    public HalfEdge halfEdge;

}
