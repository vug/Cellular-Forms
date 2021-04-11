using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsInitializer : MonoBehaviour
{
    public Parameters parameters;
    public static GameObject cellPrefab;
    private System.Random rnd;

    void Start()
    {
        cellPrefab = parameters.cellPrefab;
        rnd = new System.Random();
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

        if (Input.GetKeyDown("s"))
        {
            int ix = rnd.Next(cells.Length);
            splitCell(cells[ix]);
        }

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

    public static void splitCell(Cell parent)
    {
        float d_closest = float.MaxValue;
        int ix_closest = -1;
        for (int i = 0; i < parent.links.Count; i++)
        {
            Cell other = parent.links[i];
            float d = (other.transform.position - parent.transform.position).magnitude;
            if (d < d_closest)
            {
                ix_closest = i;
                d_closest = d;
            }
        }
        int ix_opposite = (ix_closest + parent.links.Count / 2) % parent.links.Count;
        Debug.Log("closest: " + ix_closest + ", opposite: " + ix_opposite);

        GameObject obj = Instantiate(cellPrefab);
        Cell child = obj.GetComponent<Cell>();
        child.transform.position = parent.transform.position;

        int ix_from = Mathf.Min(ix_closest, ix_opposite);
        int ix_to = Mathf.Max(ix_closest, ix_opposite);
        Debug.Log("from: " + ix_from + ", to: " + ix_to);

        Vector3 child_pos = child.transform.position;
        Vector3 parent_pos = parent.transform.position;
        for (int ix = 0; ix < parent.links.Count; ix++)
        {
            Cell cell = parent.links[ix];
            if (ix >= ix_from && ix <= ix_to)
            {
                child_pos += cell.transform.position;

                child.links.Add(cell);
                cell.links.Add(child);
                parent.links.Remove(cell);
                cell.links.Remove(parent);
            }
            else
            {
                parent_pos += cell.transform.position;
            }
        }
        child.links.Add(parent);
        parent.links.Add(child);
        Debug.Log("child links: " + child.links.Count + " parent links: " + parent.links.Count);
        child.transform.position = child_pos /= (child.links.Count + 1);
        child.updateNormal();
        parent.transform.position = parent_pos /= (parent.links.Count + 1);
        parent.updateNormal();
    }
}
