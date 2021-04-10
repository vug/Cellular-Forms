using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct DebugCellInfo
{
    public Vector3 springTarget;
}

public class Cell : MonoBehaviour
{
    public Parameters parameters;
    public List<Cell> links;
    private DebugCellInfo debugCellInfo;

    void Update()
    {
        Vector3 p = this.transform.position;

        Vector3 springTarget = Vector3.zero;
        foreach (Cell other in this.links)
        {
            Vector3 q = other.transform.position;
            springTarget += q + parameters.linkRestLength * (p - q).normalized;

        }
        springTarget /= links.Count;

        this.transform.position = p + parameters.springFactor * (springTarget - p);

        debugCellInfo.springTarget = springTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(debugCellInfo.springTarget, 0.1f);

        Gizmos.color = Color.white;
        foreach (Cell other in this.links)
        {
            Gizmos.DrawLine(this.transform.position, other.transform.position);
        }
    }
}
