using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour

{
    public static HalfEdgeMesh heMesh;

    void Awake()
    {
        heMesh = HalfEdgeMesh.makeFromFile("Assets/Meshes/icosahedron.halfedge");
        Debug.Log("HalfEdgeMesh: " + heMesh.halfEdges.Count + " " + heMesh.vertices.Count + " " + heMesh.edges.Count + " " + heMesh.faces.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
