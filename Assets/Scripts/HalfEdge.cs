using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public List<Vertex> GetNeighbors()
    {
        List<Vertex> neighbors = new List<Vertex>();
        HalfEdge h = this.halfEdge;
        int num_steps = 0;
        do
        {
            HalfEdge ht = h.twin;
            neighbors.Add(ht.vertex);
            h = ht.next;
            num_steps++;
        } while (h != null && h != this.halfEdge && num_steps < 1000);
        return neighbors;
    }

    public List<HalfEdge> GetHalfEdges()
    {
        List<HalfEdge> halfEdges = new List<HalfEdge>();
        HalfEdge h = this.halfEdge;
        int num_steps = 0;
        do
        {
            halfEdges.Add(h);
            HalfEdge ht = h.twin;
            h = ht.next;
            num_steps++;
        } while (h != null && h != this.halfEdge && num_steps < 1000);
        return halfEdges;
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

public class HalfEdgeMesh
{
    public Dictionary<int, HalfEdge> halfEdges = new Dictionary<int, HalfEdge>();
    public Dictionary<int, Vertex> vertices = new Dictionary<int, Vertex>();
    public Dictionary<int, Edge> edges = new Dictionary<int, Edge>();
    public Dictionary<int, Face> faces = new Dictionary<int, Face>();
    private int maxId = 1000;

    public HalfEdge newHalfEdge()
    {
        HalfEdge h = new HalfEdge();
        h.id = maxId;
        halfEdges[maxId] = h;
        maxId++;
        return h;
    }


    public Vertex newVertex()
    {
        Vertex v = new Vertex();
        v.id = maxId;
        vertices[maxId] = v;
        maxId++;
        return v;
    }

    public Edge newEdge()
    {
        Edge e = new Edge();
        e.id = maxId;
        edges[maxId] = e;
        maxId++;
        return e;
    }
    public Face newFace()
    {
        Face f = new Face();
        f.id = maxId;
        faces[maxId] = f;
        maxId++;
        return f;
    }


    public Mesh convertToMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] mesh_vertices = new Vector3[vertices.Count];
        int[] mesh_triangles = new int[faces.Count * 3];

        // create Mesh
        Dictionary<int, int> idMap = new Dictionary<int, int>();
        int newId = 0;
        foreach (KeyValuePair<int, Vertex> entry in vertices)
        {
            int halfEdgeId = entry.Key;
            idMap[halfEdgeId] = newId;
            mesh_vertices[newId] = entry.Value.position;
            newId++;
        }
        int faceIx = 0;
        foreach(Face f in faces.Values)
        {
            HalfEdge h = f.halfEdge;
            int vertIx = 0;
            do
            {
                Vertex v = h.vertex;
                newId = idMap[v.id];
                mesh_triangles[faceIx * 3 + vertIx] = newId;
                h = h.next;
                vertIx++;
            } while (h != f.halfEdge);
            faceIx++;
        }

        // update Mesh
        mesh.Clear();
        mesh.vertices = mesh_vertices;
        mesh.triangles = mesh_triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        return mesh;
    }

    public static HalfEdgeMesh makeFromFile(string path)
    {
        List<string> lines = new List<string>();
        HalfEdgeMesh mesh = new HalfEdgeMesh();

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
        foreach (String line in lines)
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
                float x = (float)Double.Parse(split[3]);
                float y = (float)Double.Parse(split[4]);
                float z = (float)Double.Parse(split[5]);
                mesh.vertices[id].halfEdge = mesh.halfEdges[halfEdgeId];
                mesh.vertices[id].position = new Vector3(x, y, z);
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

    // Just an example of how to create a HalfEdgeMesh with actual content
    public static HalfEdge makeTriangle()
    {
        Face f1 = new Face();

        Edge e1 = new Edge();
        Edge e2 = new Edge();
        Edge e3 = new Edge();

        Vertex v1 = new Vertex();
        Vertex v2 = new Vertex();
        Vertex v3 = new Vertex();
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
