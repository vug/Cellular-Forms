using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct DebugCellInfo
{
    public Vector3 springTarget;
    public Vector3 planarTarget;
}

public class Cell : MonoBehaviour
{
    public Parameters parameters;

    public List<Cell> links;
    public Vector3 normal;

    private DebugCellInfo debugCellInfo;
    public bool shouldDrawDebug = false;

    void Update()
    {
        Vector3 p = this.transform.position;

        Vector3 springTarget = Vector3.zero;
        Vector3 planarTarget = Vector3.zero;
        foreach (Cell other in this.links)
        {
            Vector3 q = other.transform.position;
            springTarget += q + parameters.linkRestLength * (p - q).normalized;
            planarTarget += q;
        }
        springTarget /= links.Count;
        planarTarget /= links.Count;

        this.transform.position = p + parameters.springFactor * (springTarget - p) + parameters.planarFactor * (planarTarget - p);

        debugCellInfo.springTarget = springTarget;
        debugCellInfo.planarTarget = planarTarget;
        if (Input.GetKeyDown("d"))
        {
            shouldDrawDebug = true;
        }
        if (Input.GetKeyDown("f"))
        {
            shouldDrawDebug = false;
        }
    }

    public void updateNormal()
    {
        Vector3 p0 = transform.position;
        Vector3 p1 = links[links.Count - 1].transform.position;
        Vector3 triangle_normals_total = Vector3.zero;
        foreach (Cell cell in links)
        {
            Vector3 p2 = cell.transform.position;
            triangle_normals_total += Vector3.Cross(p1 - p0, p2 - p0).normalized;
            p1 = p2;
        }
        normal = triangle_normals_total.normalized;
    }

    private void OnDrawGizmos()
    {
        if (!shouldDrawDebug) { return; }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(debugCellInfo.springTarget, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugCellInfo.planarTarget, 0.1f);

        Gizmos.color = Color.white;
        foreach (Cell other in links)
        {
            Gizmos.DrawLine(transform.position, other.transform.position);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + normal);
    }
}
