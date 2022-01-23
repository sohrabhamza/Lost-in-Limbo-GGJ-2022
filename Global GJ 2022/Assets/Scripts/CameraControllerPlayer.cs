using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPlayer : MonoBehaviour
{
    public bool isEnabled;
    [SerializeField] Transform target;
    [SerializeField] float distance, height;
    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;

        transform.position = target.TransformPoint(0, height, -distance);
    }
}
