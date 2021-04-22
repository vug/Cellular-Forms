using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    public Parameters parameters;
    public static GameObject cellPrefab;
    private Dictionary<int, GameObject> cellObjs;
    private HalfEdgeMesh heMesh;


    void Start()
    {
        this.heMesh = Main.heMesh;
        cellPrefab = parameters.cellPrefab;
        cellObjs = new Dictionary<int, GameObject>();
    }

    void Update()
    {
        var keys = new List<int>(cellObjs.Keys);
        foreach (var ix in keys)
        {
            if(!heMesh.vertices.ContainsKey(ix))
            {
                var obj = cellObjs[ix];
                cellObjs.Remove(ix);
                Destroy(obj);
            }
        }

        foreach (var entry in heMesh.vertices)
        {
            if (!cellObjs.ContainsKey(entry.Key))
            {
                cellObjs[entry.Key] = Instantiate(cellPrefab);
            }
        }

        foreach (int ix in heMesh.vertices.Keys)
        {
            Cell cell = cellObjs[ix].GetComponent<Cell>();
            cell.transform.position = heMesh.vertices[ix].position;
        }
    }

    public Cell[] makeTetrahedron()
    {
        Vector3[] vertices = new[] {
            new Vector3(Mathf.Sqrt(8.0f / 9.0f), 0, -1.0f / 3.0f),
            new Vector3(-Mathf.Sqrt(2.0f / 9.0f), Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f),
            new Vector3(-Mathf.Sqrt(2.0f / 9.0f), -Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f),
            new Vector3(0, 0, 1),
        };
        var links = new Dictionary<int, int[]>
        {
            [0] = new int[] { 1, 3, 2 }, // { 1, 2, 3 } was inwards
            [1] = new int[] { 2, 3, 0 },
            [2] = new int[] { 0, 3, 1 }, // { 0, 1, 3 } was inwards
            [3] = new int[] { 0, 1, 2 },
        };


        Cell[] cells = new Cell[4];
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = Instantiate(cellPrefab);
            cells[i] = obj.GetComponent<Cell>();
            cells[i].transform.position = vertices[i];
        }
        for (int i = 0; i < 4; i++)
        {
            foreach (int j in links[i])
            {
                cells[i].links.Add(cells[j]);
            }
        }

        return cells;
    }
}
