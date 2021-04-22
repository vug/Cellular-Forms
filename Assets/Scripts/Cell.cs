using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct DebugCellInfo
{
    public Vector3 springTarget;
    public Vector3 planarTarget;
    public Vector3 bulgeTarget;
}

public class Cell : MonoBehaviour
{
    public Parameters parameters;

    public List<Cell> links;
    public Vector3 normal;

    private DebugCellInfo debugCellInfo;
    public bool shouldDrawDebug = true;

    void Update()
    {
        //debugCellInfo.springTarget = springTarget;
        //debugCellInfo.planarTarget = planarTarget;
        //debugCellInfo.bulgeTarget = bulgeTarget;

        //if (Input.GetKeyDown("d"))
        //{
        //    shouldDrawDebug = true;
        //}
        //if (Input.GetKeyDown("f"))
        //{
        //    shouldDrawDebug = false;
        //}
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (!shouldDrawDebug) { return; }
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(debugCellInfo.springTarget, 0.1f);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(debugCellInfo.planarTarget, 0.1f);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(debugCellInfo.bulgeTarget, 0.1f);

    //    Gizmos.color = Color.white;
    //    //foreach (Cell other in links)
    //    for (int ix = 0; ix < links.Count; ix++)
    //    {
    //        Cell other = links[ix];
    //        Gizmos.DrawLine(transform.position, other.transform.position);
    //        GUIStyle style = new GUIStyle();
    //        style.normal.textColor = Color.red;
    //        UnityEditor.Handles.Label(other.transform.position, "" + ix, style);
    //    }

    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawLine(transform.position, transform.position + normal);
    //}
}
