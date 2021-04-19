using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HalfEdgeStudy : MonoBehaviour
{
    void Start()
    {
        Debug.Log("HalfEdgeStudy. Traversing...");
        HalfEdge h = MeshGenerator.makeTriangle();
        Face f = h.face;
        f.traverseVertices();
    }
}

public class HalfEdge
{
    public int id = -1;
    public HalfEdge next;
    public HalfEdge twin;
    public Vertex vertex;
    public Edge edge;
    public Face face;
}

public class Vertex
{
    public int id = -1;
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
    public int id = -1;
    public HalfEdge halfEdge;
}

public class Face
{
    public int id = -1;
    public HalfEdge halfEdge;

    public void traverseVertices()
    {
        HalfEdge h = this.halfEdge;
        do
        {
            Vertex v = h.vertex;
            Debug.Log("v[" + v.id + "] at " + v.position);
            h = h.next;
        } while (h != this.halfEdge);
    }
}

public class Mesh
{
    public Dictionary<int, HalfEdge> halfEdges = new Dictionary<int, HalfEdge>();
    public Dictionary<int, Vertex> vertices = new Dictionary<int, Vertex>();
    public Dictionary<int, Edge> edges = new Dictionary<int, Edge>();
    public Dictionary<int, Face> faces = new Dictionary<int, Face>();
}

public class MeshGenerator
{
    public static Mesh readHalfEdge(string path)
    {
        List<string> lines = new List<string>();
        Mesh mesh = new Mesh();

        try
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        // First pass, create empty elements
        foreach(String line in lines)
        {
            int id = Int32.Parse(line.Split(' ')[1]);
            if (line.StartsWith("halfedge"))
            {
                mesh.halfEdges[id] = new HalfEdge { id = id };
            }
            else if (line.StartsWith("vertex"))
            {
                mesh.vertices[id] = new Vertex { id = id };
            }
            else if (line.StartsWith("edge"))
            {
                mesh.edges[id] = new Edge { id = id };
            }
            else if (line.StartsWith("face"))
            {
                mesh.faces[id] = new Face { id = id };
            }
        }

        // Second pass, set connections
        foreach (String line in lines)
        {
            string[] split = line.Split(' ');

            int id = Int32.Parse(split[1]);

            if (line.StartsWith("halfedge"))
            {
                int nextHalfEdgeId = Int32.Parse(split[2]);
                int twinHalfEdgeId = Int32.Parse(split[3]);
                int vertexId = Int32.Parse(split[4]);
                int edgeId = Int32.Parse(split[5]);
                int faceId = Int32.Parse(split[6]);

                mesh.halfEdges[id].next = mesh.halfEdges[nextHalfEdgeId];
                mesh.halfEdges[id].twin = mesh.halfEdges[twinHalfEdgeId];
                mesh.halfEdges[id].vertex = mesh.vertices[vertexId];
                mesh.halfEdges[id].edge = mesh.edges[edgeId];
                mesh.halfEdges[id].face = mesh.faces[faceId];
            }
            else if (line.StartsWith("vertex"))
            {
                int halfEdgeId = Int32.Parse(split[2]);
                mesh.vertices[id].halfEdge = mesh.halfEdges[halfEdgeId];
                // TODO: position comes here
                mesh.vertices[id].position = Vector3.zero;
            }
            else if (line.StartsWith("edge"))
            {
                int halfEdgeId = Int32.Parse(split[2]);
                mesh.edges[id].halfEdge = mesh.halfEdges[halfEdgeId];

            }
            else if (line.StartsWith("face"))
            {
                int halfEdgeId = Int32.Parse(split[2]);
                mesh.faces[id].halfEdge = mesh.halfEdges[halfEdgeId];
            }
        }

        return mesh;
    }

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
