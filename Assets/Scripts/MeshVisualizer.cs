using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVisualizer : MonoBehaviour
{
    private HalfEdgeMesh heMesh;
    public static MeshFilter meshFilter;

    void Start()
    {
        this.heMesh = Main.heMesh;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = heMesh.convertToMesh();
    }

    void Update()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        // TODO: when called from Update below heMesh.convertToMesh() generates inside-out faces.
        // Therefor I use a "double-faced" material.
        meshFilter.mesh = heMesh.convertToMesh();
    }
}
