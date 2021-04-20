using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerationStudy : MonoBehaviour
{
    void Start()
    {
        HalfEdgeMesh heMesh = HalfEdgeMeshGenerator.readHalfEdge("Assets/Meshes/icosahedron.halfedge");
        Debug.Log("HalfEdgeMesh: " + heMesh.halfEdges.Count + " " + heMesh.vertices.Count + " " + heMesh.edges.Count + " " + heMesh.faces.Count);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = heMesh.convertToMesh();
        Debug.Log("UnityMesh: " + meshFilter.mesh.vertices.Length + " " + meshFilter.mesh.triangles.Length);
        foreach(Vector3 v in meshFilter.mesh.vertices)
        {
            Debug.Log("vertex: " + v);
        }
        Debug.Log("triangles: " + String.Join(",", meshFilter.mesh.triangles));
    }
}
