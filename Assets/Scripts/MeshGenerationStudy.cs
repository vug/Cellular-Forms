using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerationStudy : MonoBehaviour
{
    public static HalfEdgeMesh heMesh;
    public static MeshFilter meshFilter;

    void Start()
    {
        heMesh = HalfEdgeMesh.makeFromFile("Assets/Meshes/icosahedron.halfedge");
        Debug.Log("HalfEdgeMesh: " + heMesh.halfEdges.Count + " " + heMesh.vertices.Count + " " + heMesh.edges.Count + " " + heMesh.faces.Count);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = heMesh.convertToMesh();
    }

    void Update()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        // TODO: when called from Update below heMesh.convertToMesh() generates inside-out faces.
        //meshFilter.mesh = heMesh.convertToMesh();
        //Array.Reverse(meshFilter.mesh.triangles, 0, meshFilter.mesh.triangles.Length);
        Vector3[] vertices = meshFilter.mesh.vertices;
        
        // Update existing mesh vertices instead of creating a new Mesh from scratch.
        int newId = 0;
        foreach (Vertex v in heMesh.vertices.Values)
        {
            vertices[newId] = v.position;
            newId++;
        }
        meshFilter.mesh.vertices = vertices;
    }
}
