using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsInitializer : MonoBehaviour
{
    public GameObject cellPrefab;

    void Start()
    {
        Cell[] cells = makeTetrahedron();
    }

    public Cell[] makeTetrahedron()
    {
        Vector3[] vertices = new[] {
            new Vector3(Mathf.Sqrt(8.0f / 9.0f), 0, -1.0f / 3.0f),
            new Vector3(- Mathf.Sqrt(2.0f / 9.0f), Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f),
            new Vector3(-Mathf.Sqrt(2.0f / 9.0f), - Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f),
            new Vector3(0, 0, 1),
        };

        Cell[] cells = new Cell[4];
        for(int i=0; i<4; i++)
        {
            GameObject obj = Instantiate(this.cellPrefab);
            cells[i] = obj.GetComponent<Cell>();
            cells[i].transform.position = vertices[i];
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (i == j) { continue; }
                cells[i].links.Add(cells[j]);
            }
        }
        return cells;
    }
}
