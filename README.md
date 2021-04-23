# Cellular Forms

A reproduction of [Andy Lomas](https://andylomas.com/)'s "Cellular Forms" work in Unity as explained in this [paper](https://andylomas.com/extra/andylomas_paper_cellular_forms_aisb50.pdf).

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


## v0.5 Bulge "Force" and Random Cell Division

Failed attempt as cell division. The child cell and parent cell are not sharing linked cells in a nice way, it turns into spaghetti. Lol. Need to learn "vertex split" algorithm and Half-edge data structure etc. first.

![Cellular Forms - 02 - Tetrahedron - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal _DX11_ 2021-04-10 22-59-59_Trim (1)](https://user-images.githubusercontent.com/6636020/114338424-0a105280-9b21-11eb-868a-13c384fce611.gif)
