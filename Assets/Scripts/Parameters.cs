using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Simulation Parameters")]
public class Parameters : ScriptableObject
{
    [Header("Simulation")]
    public float linkRestLength;
    public float springFactor;
    public float planarFactor;
    public float bulgeFactor;

    [Header("Visualization")]
    public Color cellColor;
}
