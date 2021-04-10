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

    private void OnDrawGizmos()
    {
        if (!shouldDrawDebug) { return; }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(debugCellInfo.springTarget, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debugCellInfo.planarTarget, 0.1f);

        Gizmos.color = Color.white;
        foreach (Cell other in this.links)
        {
            Gizmos.DrawLine(this.transform.position, other.transform.position);
        }
    }
}
