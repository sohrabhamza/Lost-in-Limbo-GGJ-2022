using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerPlayer : MonoBehaviour
{
    public bool isEnabled;
    [SerializeField] Transform target1;
    [SerializeField] Transform target2;

    [SerializeField] Vector3 offset;
    [SerializeField] float lerpRate;

    private Transform currentTarget;

    // Start is called once
    private void Start() 
    {
        currentTarget = target1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SwitchTarget();
            }

            transform.position = Vector3.Lerp(transform.position, currentTarget.position + offset, Time.deltaTime * lerpRate);
        }
    }

    private void SwitchTarget() 
    {
        currentTarget = currentTarget == target1 ? target2 : target1; 
    }
}
