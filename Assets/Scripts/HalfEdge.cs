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

    public Edge VertexSplit(Vertex v0, HalfEdge h1, HalfEdge h2, bool isThree = false)
    {
        Debug.Log("splitting vertex[" + v0.id + "] between halfedge[" + h1.id + "] and [" + h2.id + "]");
        // Phase I: label existing elements
        HalfEdge h3 = h1.twin;
        HalfEdge h4 = h2.twin;

        Vertex v1 = h3.vertex;
        Vertex v2 = h4.vertex;

        Edge e1 = h1.edge;
        Edge e2 = h2.edge;

        // Phase II: 2 new vertices, 5 new edges, 10 new half-edges, 2 new faces
        Vertex v3 = newVertex();
        Vertex v4 = newVertex();

        Edge e3 = newEdge();
        Edge e4 = newEdge();
        Edge e5 = newEdge();
        Edge e6 = newEdge();
        Edge e7 = newEdge();

        HalfEdge h5 = newHalfEdge();
        HalfEdge h6 = newHalfEdge();
        HalfEdge h7 = newHalfEdge();
        HalfEdge h8 = newHalfEdge();
        HalfEdge h9 = newHalfEdge();
        HalfEdge h10 = newHalfEdge();
        HalfEdge h11 = newHalfEdge();
        HalfEdge h12 = newHalfEdge();
        HalfEdge h13 = newHalfEdge();
        HalfEdge h14 = newHalfEdge();

        Face f0 = newFace();
        Face f1 = newFace();

        // Phase III: Make connections
        h5.next = h6;
        h5.twin = h8;
        h5.vertex = v3;
        h5.edge = e3;
        h5.face = f0;

        h6.next = h7;
        h6.twin = h11;
        h6.vertex = v4;
        h6.edge = e4;
        h6.face = f0;

        h7.next = h5;
        h7.twin = h12;
        h7.vertex = v1;
        h7.edge = e5;
        h7.face = f0;

        h8.next = h9;
        h8.twin = h5;
        h8.vertex = v4;
        h8.edge = e3;
        h8.face = f1;

        h9.next = h10;
        h9.twin = h13;
        h9.vertex = v3;
        h9.edge = e6;
        h9.face = f1;

        h10.next = h8;
        h10.twin = h14;
        h10.vertex = v2;
        h10.edge = e7;
        h10.face = f1;

        if (isThree)
        {
            h11.next = h14;
            h11.twin = h6;
            h11.vertex = v1;
            h11.edge = e4;
            h11.face = h3.face;

        }
        else
        {
            h11.next = h3.next;
            h11.twin = h6;
            h11.vertex = v1;
            h11.edge = e4;
            h11.face = h3.face;
        }

        h12.next = h1.next;
        h12.twin = h7;
        h12.vertex = v3;
        h12.edge = e5;
        h12.face = h1.face;

        h13.next = h4.next;
        h13.twin = h9;
        h13.vertex = v2;
        h13.edge = e6;
        h13.face = h4.face;

        h14.next = h2.next;
        h14.twin = h10;
        h14.vertex = v4;
        h14.edge = e7;
        h14.face = h2.face;

        v1.halfEdge = h7;
        v2.halfEdge = h10;
        v3.halfEdge = h5;
        v3.position = v0.position;
        v4.halfEdge = h8;
        v4.position = v0.position;

        e3.halfEdge = h5;
        e4.halfEdge = h6;
        e5.halfEdge = h7;
        e6.halfEdge = h9;
        e7.halfEdge = h10;

        f0.halfEdge = h5;
        f1.halfEdge = h8;


        // Loops
        HalfEdge h;
        int cnt;
        h = h4;
        cnt = 1;
        int num_loops = 0;
        do
        {
            Debug.Log("he[" + h.id + "]" + " next: [" + h.next.id + "], twin: [" + h.twin.id + "]");
            h = h.next; // outgoing edge from v0
            h.vertex = v3;
            h = h.twin; // ingoing edge to v0
            if (h.vertex != v1 && h.vertex != v2)
            {
                v3.position += h.vertex.position;
                cnt += 1;
            }
            num_loops++;
            //} while(h != h3);
            //} while(h != h3 || h.vertex() != v1);
        } while (h.next.twin != h3 && num_loops < 10);
        //v3.pos += v1.pos + v2.pos;
        v3.position /= (float)cnt;
        Debug.Log("num_loops: " + num_loops);

        if (!isThree)
        {
            h = h3;
            cnt = 1;
            num_loops = 0;
            do
            {
                h = h.next; // outgoing edge from v0
                h.vertex = v4;
                h = h.twin;
                if (h.vertex != v1 && h.vertex != v2)
                {
                    v4.position += h.vertex.position;
                    cnt += 1;
                }
                num_loops++;
                //} while(h != h4 || h.vertex() != v2);
            } while (h.next.twin != h4 && num_loops < 10);
            // v4.pos += v1.pos + v2.pos;
            v4.position /= (float)cnt;
            Debug.Log("num_loops: " + num_loops);
        }

        h1.next.next.next = h12;
        h1.face.halfEdge = h12;
        h2.next.next.next = h14;
        h2.face.halfEdge = h14;
        h3.next.next.next = h11;
        h3.face.halfEdge = h11;
        h4.next.next.next = h13;
        h4.face.halfEdge = h13;

        // Phase 4: delete unused, unreferred elements
        edges.Remove(e1.id);
        edges.Remove(e2.id);

        vertices.Remove(v0.id);

        halfEdges.Remove(h1.id);
        halfEdges.Remove(h2.id);
        halfEdges.Remove(h3.id);
        halfEdges.Remove(h4.id);

        return e3;
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
