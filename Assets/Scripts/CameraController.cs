using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Vector3.zero);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
