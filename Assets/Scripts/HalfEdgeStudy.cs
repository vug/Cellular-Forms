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

    // Only works if not on boundary
    public void traverseNeighbors()
    {
        HalfEdge h = this.halfEdge;
        do
        {
            h = h.twin;
            Vertex v = h.vertex;
            Debug.Log(v.position);
            h = h.next;
        } while (h != null || h != this.halfEdge);
    }
}

public class Edge
{
    public HalfEdge halfEdge;
}

public class Face
{
    public HalfEdge halfEdge;

    public void traverseVertices()
    {
        HalfEdge h = this.halfEdge;
        do
        {
            Vertex v = h.vertex;
            Debug.Log(v.position);
            h = h.next;
        } while (h != this.halfEdge);
    }
}

public class MeshGenerator
{
    public static HalfEdge makeTriangle()
    {
        Face f1 = new Face();

        Edge e1 = new Edge();
        Edge e2 = new Edge();
        Edge e3 = new Edge();

        Vertex v1 = new Vertex();
        Vertex v2 = new Vertex();
        Vertex v3 =  new Vertex();
        v1.position = new Vector3(0.0f, 0.0f, 0.0f);
        v2.position = new Vector3(1.0f, 0.0f, 0.0f);
        v3.position = new Vector3(0.5f, 1.0f, 0.0f);

        HalfEdge h1 = new HalfEdge();
        HalfEdge h2 = new HalfEdge();
        HalfEdge h3 = new HalfEdge();
        HalfEdge h4 = new HalfEdge();
        HalfEdge h5 = new HalfEdge();
        HalfEdge h6 = new HalfEdge();

        f1.halfEdge = h1;

        e1.halfEdge = h1;
        e2.halfEdge = h2;
        e3.halfEdge = h3;

        v1.halfEdge = h1;
        v2.halfEdge = h2;
        v3.halfEdge = h3;

        h1.next = h2;
        h1.twin = h4;
        h1.vertex = v1;
        h1.edge = e1;
        h1.face = f1;

        h2.next = h3;
        h2.twin = h6;
        h2.vertex = v2;
        h2.edge = e2;
        h2.face = f1;

        h3.next = h1;
        h3.twin = h5;
        h3.vertex = v3;
        h3.edge = e3;
        h3.face = f1;

        h4.next = null;
        h4.twin = h1;
        h4.vertex = v2;
        h4.edge = e1;
        h4.face = null;

        h5.next = null;
        h5.twin = h3;
        h5.vertex = v1;
        h5.edge = e3;
        h5.face = null;

        h6.next = null;
        h6.twin = h2;
        h6.vertex = v3;
        h6.edge = e2;
        h6.face = null;

        return h1;
    }
}
