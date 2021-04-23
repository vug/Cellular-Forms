# Cellular Forms

A reproduction of [Andy Lomas](https://andylomas.com/)'s "Cellular Forms" work in Unity as explained in this [paper](https://andylomas.com/extra/andylomas_paper_cellular_forms_aisb50.pdf).

## v0.9 Repulsion Force

This was the last inter-cellular force mention in the paper. Cells that are closers than certain distance, and are not linked/connected (vertices connected via edge) exert a force on each other that keeps the volume of the tissue. Otherwise the tissue tends to collapse on itself.

![Cellular Forms - 04 - Repulsion - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal _DX11_ 2021-04-23 00-38-40_Trim](https://user-images.githubusercontent.com/6636020/115820140-3b650a00-a3ce-11eb-9ea4-8446a377a990.gif)

Current implementation is unfortunately a basic double loop which is unfortunately `O(N^2)` with the number of cells. 

## v0.8 Vertex Split with HalfEdgeMesh. Mesh and Cell visualizations follow HalfEdge.

Unlike v0.5, this time neighborhood/connection/link structure does not turn into spaghetti, but preserve local connections, which keeps the mesh as triangular mesh while the tissue is growing! This was a big milestone. ^_^

![Cellular Forms - 03 - HalfEdge - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal_ _DX11_ 2021-04-22 01-55-47_Trim](https://user-images.githubusercontent.com/6636020/115663595-c088ea00-a30e-11eb-8146-755f7309048f.gif)

Also majorly refactored the structure. Previously `UnityMesh` was following `HalfEdgeMesh`, whereas `HalfEdgeMesh` was following changes in `Cell`s (and they were reading same `.halfedge` file separately) and each class had their own updates etc. 👎 

This time we have a proper Main class that reads the `.halfedge` file. `HalfEdgeMesh` is its member which is the source of truth about the tissue. Mesh Visualizer and Cell Visualizer access this same source to do different visualizations (former a flat shaded triangular mesh, latter spheres at vertices).

Also moved the simulation logic to `Main` class. So, individual Cell instances does not compute things separately by themselves. This allowed to compute new positions first, and update all of them later. Which prevents drifts (common problem in badly coded physics simulations).

Also added sphere/cell radius as a new visualization parameter.


## v0.7 Creating a Unity Mesh from Half Edge Structure and Visualizing via Flat Shading Material

Unity comes with a Mesh class that works with an array of vertices and an array of triangle corner indices. This version converts half edge to unity mesh. And also visualizes it with a flat shading material that required installation of Universal Rendering Pipeline (URL) and Shader Graph packages. 

![Cellular Forms - 03 - HalfEdge - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal_ _DX11_ 2021-04-20 02-10-07_Trim](https://user-images.githubusercontent.com/6636020/115346136-c7342780-a17d-11eb-9f0c-fa689f0c2a81.gif)
