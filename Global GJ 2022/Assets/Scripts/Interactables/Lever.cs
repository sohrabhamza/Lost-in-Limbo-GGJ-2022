using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Transform shaft;
    [SerializeField] UnityEvent offEvent;
    [SerializeField] UnityEvent onEvent;
    [SerializeField] UnityEvent changeEvent;

    HingeJoint2D hinge;
    float motorSpeed;
    bool state;
    bool lastState;

    private void Start()
    {
        hinge = GetComponentInChildren<HingeJoint2D>();
        motorSpeed = hinge.motor.motorSpeed;
    }

    private void Update()
    {
        float angle = shaft.localEulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        if (Mathf.Abs(angle - hinge.limits.min) < 3)
        {
            state = true;
        }
        else if (Mathf.Abs(angle - hinge.limits.max) < 3)
        {
            state = false;
        }


        if (Mathf.Abs(angle - hinge.limits.min) < Mathf.Abs(angle - hinge.limits.max))
        {
            JointMotor2D motor = hinge.motor;
            motor.motorSpeed = -motorSpeed;
            hinge.motor = motor;
        }
        else
        {
            JointMotor2D motor = hinge.motor;
            motor.motorSpeed = motorSpeed;
            hinge.motor = motor;
        }

        if (lastState != state)
        {
            if (state) onEvent.Invoke(); else offEvent.Invoke();
            changeEvent.Invoke();
        }

        lastState = state;
    }
}
