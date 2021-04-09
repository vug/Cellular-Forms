using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Parameters parameters;
    public List<Cell> links;

    void Start()
    {
    }

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
    }
}
