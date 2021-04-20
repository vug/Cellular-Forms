using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerationStudy : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

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

        //mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        //CreateShape();
        //UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2,
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
