using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    void Update()
    {
        float v = 0.25f;
        transform.position = new Vector3(Mathf.Cos(v * Time.time), 0, Mathf.Sin(v * Time.time)) * 10;
        transform.LookAt(Vector3.zero); //, new Vector3(0, 0, 1));
    }
}
