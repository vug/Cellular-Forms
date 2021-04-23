# Cellular Forms

A reproduction of [Andy Lomas](https://andylomas.com/)'s "Cellular Forms" work in Unity as explained in this [paper](https://andylomas.com/extra/andylomas_paper_cellular_forms_aisb50.pdf).

See https://github.com/vug/cellular-forms/releases for progress history.

## v0.10 Random nutrition based division

Each cell gets some random amount of nutrition with a frame rate independent rate. When accumulated nutrition in a cell hits a threshold cell splits/divides.

![Cellular Forms - 04 - Repulsion - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal _DX11_ 2021-04-23 00-57-18_Trim](https://user-images.githubusercontent.com/6636020/115820869-b37fff80-a3cf-11eb-9f27-566f0179dab8.gif)

Currently number of cells can go up to 750 on my machine down to a frame rate 20FPS. Need optimization!

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
