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
    public float radiusOfInfluence;
    public float repulsionStrength; 

    [Header("Visualization")]
    public GameObject cellPrefab;
    public Color cellColor;
    public float cellRadius;
    public float cameraDistance;
}
