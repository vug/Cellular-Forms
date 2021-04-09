using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(Mathf.Cos(Time.time), 0, Mathf.Sin(Time.time)) * 5;
        transform.LookAt(Vector3.zero); //, new Vector3(0, 0, 1));
    }
}
