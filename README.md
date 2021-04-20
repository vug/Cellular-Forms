# Cellular Forms

A reproduction of [Andy Lomas](https://andylomas.com/)'s "Cellular Forms" work in Unity as explained in this [paper](https://andylomas.com/extra/andylomas_paper_cellular_forms_aisb50.pdf).

## v0.7 Creating a Unity Mesh from Half Edge Structure and Visualizing via Flat Shading Material

Unity comes with a Mesh class that works with an array of vertices and an array of triangle corner indices. This version converts half edge to unity mesh. And also visualizes it with a flat shading material that required installation of Universal Rendering Pipeline (URL) and Shader Graph packages. 

![Cellular Forms - 03 - HalfEdge - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal_ _DX11_ 2021-04-20 02-10-07_Trim](https://user-images.githubusercontent.com/6636020/115346136-c7342780-a17d-11eb-9f0c-fa689f0c2a81.gif)

## v0.6 Half Edge Data Structure and reading it from a file

In order to implement vertex split properly (something I failed spectacularly at v0.5) I need to learn how to do "Vertex Split" properly. Looks vertex split is the computer graphics technical term for the "cell division" algorithm described in Andy Lomas' paper. And I learned that a good data structure to implement local mesh operations on triangular meshes (or polygonal meshes in general) is the half edge data structure.

I implemented one in CSharp. I deviced a methodology to create a mesh in Blender (platonic solids are good for this project) import it to `dae` format. Load it in my Scotty3D in which I've implemented vertex split (and edge collapse and edge flip) and serialize the half edge in a custom text-based format. Then load it in this Unity project.

Below is the result of loading a icosahedron generated in Blender, serialized in Scotty3D, loaded in Unity. From each mesh vertex a Cell (sphere) is generated. And Cells are linked according to vertex edge connections. ^_^

![Cellular Forms - 03 - HalfEdge - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal_ _DX11_ 2021-04-19 20-48-24_Trim](https://user-images.githubusercontent.com/6636020/115321029-fb452380-a150-11eb-943a-b795e868a07a.gif)


## v0.5 Bulge "Force" and Random Cell Division

Failed attempt as cell division. The child cell and parent cell are not sharing linked cells in a nice way, it turns into spaghetti. Lol. Need to learn "vertex split" algorithm and Half-edge data structure etc. first.

![Cellular Forms - 02 - Tetrahedron - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal _DX11_ 2021-04-10 22-59-59_Trim (1)](https://user-images.githubusercontent.com/6636020/114338424-0a105280-9b21-11eb-868a-13c384fce611.gif)

## v0.4: Planar "force"

Spring "force" was pushing or pulling linked cells to spring's rest length. Planar force pushes a cell towards to center of mass of linked cells. It'll iron out the wrinkles and flatten out surfaces.

Playing with the weight of each force moves cells to different equilibrium points.

![Cellular Forms - 02 - Tetrahedron - PC, Mac  Linux Standalone - Unity 2020 3 3f1 Personal _DX11_ 2021-04-10 16-34-31_Trim](https://user-images.githubusercontent.com/6636020/114284648-452a5d00-9a1f-11eb-927f-56ac918cef3a.gif)
