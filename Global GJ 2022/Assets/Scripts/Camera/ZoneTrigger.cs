using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoneTrigger : MonoBehaviour
{
    bool fireEnter;
    bool waterEnter;
    bool isEnable;
    bool isDisable = true;
    CinemachineSwitcher cinemachineSwitcher;
    CinemachineVirtualCamera myCam;

    private void Start()
    {
        cinemachineSwitcher = FindObjectOfType<CinemachineSwitcher>();
        myCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            fireEnter = true;
        }
        if (other.gameObject.layer == 7)
        {
            waterEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            fireEnter = false;
        }
        if (other.gameObject.layer == 7)
        {
            waterEnter = false;
        }
    }

    private void Update()
    {
        if (!isEnable && fireEnter && waterEnter)
        {
            cinemachineSwitcher.ChangePriority(myCam);
            isEnable = true;
            isDisable = false;
        }

        if ((!fireEnter || !waterEnter) && !isDisable)
        {
            isEnable = false;
            isDisable = true;
            cinemachineSwitcher.SwitchBack();
        }
    }
}
