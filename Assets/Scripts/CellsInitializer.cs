using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsInitializer : MonoBehaviour
{
    public Parameters parameters;
    public static GameObject cellPrefab;

    void Start()
    {
        cellPrefab = parameters.cellPrefab;
        Cell[] cells = makeTetrahedron();
        foreach (Cell cell in cells)
        {
            cell.updateNormal();
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
            GameObject obj = Instantiate(this.cellPrefab);
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

    void Update()
    {
        Cell[] cells = FindObjectsOfType<Cell>();

        Vector3 center = Vector3.zero;
        foreach (Cell cell in cells)
        {
            center += cell.transform.position;
        }
        center /= cells.Length;
        foreach (Cell cell in cells)
        {
            cell.transform.position -= center;
        }

    }
}
