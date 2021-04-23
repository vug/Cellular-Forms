using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private System.Random rnd;
    public Parameters parameters;
    public static HalfEdgeMesh heMesh;

    void Awake()
    {
        rnd = new System.Random();
        //heMesh = HalfEdgeMesh.makeFromFile("Assets/Meshes/tetrahedron.halfedge");
        heMesh = HalfEdgeMesh.makeFromFile("Assets/Meshes/icosahedron.halfedge");
        //heMesh = HalfEdgeMesh.makeFromFile("Assets/Meshes/pentakis_decodahedron.halfedge");
        Debug.Log("HalfEdgeMesh: " + heMesh.halfEdges.Count + " " + heMesh.vertices.Count + " " + heMesh.edges.Count + " " + heMesh.faces.Count);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCellPositions();

        if (Input.GetKeyDown("s"))
        {
            var vertexIds = new List<int>(heMesh.vertices.Keys);
            int vId = vertexIds[rnd.Next(vertexIds.Count)];
            Vertex randomVertex = heMesh.vertices[vId];
            List<HalfEdge> halfEdges = randomVertex.GetHalfEdges();
            int ix1 = rnd.Next(halfEdges.Count);
            int ix2 = (ix1 + halfEdges.Count / 2) % halfEdges.Count;
            heMesh.VertexSplit(randomVertex, halfEdges[ix1], halfEdges[ix2]);
        }
    }

    void UpdateCellPositions()
    {
        // Compute new positions without updating first
        Dictionary<int, Vector3> newPositions = new Dictionary<int, Vector3>();
        Vector3 center = Vector3.zero;
        foreach(KeyValuePair<int, Vertex> entry in heMesh.vertices)
        {
            center += newPositions[entry.Key] = NewCellPosition(entry.Value);
        }
        center /= heMesh.vertices.Count;

        // Update positions
        foreach (KeyValuePair<int, Vertex> entry in heMesh.vertices)
        {
            entry.Value.position = newPositions[entry.Key] - center;
        }
    }

    Vector3 NewCellPosition(Vertex v)
    {
        Vector3 p = v.position;

        Vector3 springTarget = Vector3.zero;
        Vector3 planarTarget = Vector3.zero;
        Vector3 collisionOffset = Vector3.zero;

        float bulgeDist = 0.0f;
        List<Vertex> links = v.GetNeighbors();
        Vector3 normal = GetCellNormal(v, links);
        foreach (Vertex neighbor in links)
        {
            Vector3 q = neighbor.position;
            Vector3 d = q - p;
            springTarget += q + parameters.linkRestLength * -d.normalized;
            planarTarget += q;

            float dSqr = d.sqrMagnitude;
            float rSqr = parameters.linkRestLength * parameters.linkRestLength;
            if (dSqr < rSqr) // can't push if too far away
            {
                float dot = Vector3.Dot(d, normal);
                bulgeDist += Mathf.Sqrt(rSqr - dSqr + dot * dot) + dot;
            }
        }
        float roi2 = parameters.radiusOfInfluence * parameters.radiusOfInfluence;
        foreach (Vertex u in heMesh.vertices.Values)
        {
            if (u == v || links.Contains(u) || (u.position - v.position).sqrMagnitude > roi2)
            {
                continue;
            }
            Vector3 d = p - u.position;
            collisionOffset += (1 - d.sqrMagnitude / roi2) * d.normalized;
        }
        springTarget /= links.Count;
        planarTarget /= links.Count;
        Vector3 bulgeTarget = p + normal * (bulgeDist / links.Count);

        p = p
            + parameters.springFactor * (springTarget - p)
            + parameters.planarFactor * (planarTarget - p)
            + parameters.bulgeFactor * (bulgeTarget - p)
            + parameters.repulsionStrength * collisionOffset;

        //p += Random.insideUnitSphere * 0.01f;
        return p;
    }

    // This might be different than actual mesh normal
    Vector3 GetCellNormal(Vertex v, List<Vertex> links)
    {
        Vector3 p0 = v.position;
        Vector3 p1 = links[links.Count - 1].position;
        Vector3 triangle_normals_total = Vector3.zero;
        foreach (Vertex w in links)
        {
            Vector3 p2 = v.position;
            triangle_normals_total += Vector3.Cross(p1 - p0, p2 - p0).normalized;
            p1 = p2;
        }
        return triangle_normals_total.normalized;
    }
}
