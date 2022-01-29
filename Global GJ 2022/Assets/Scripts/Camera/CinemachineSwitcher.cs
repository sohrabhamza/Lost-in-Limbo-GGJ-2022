using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;
    CinemachineVirtualCamera[] virtualCameras;
    [HideInInspector] public bool isCurrentlyZoomedOut;

    private void Awake()
    {

    }
    private void Start()
    {
        virtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
    }

    public void ChangePriority(CinemachineVirtualCamera vc)
    {
        foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
        {
            if (virtualCamera != vc)
            {
                virtualCamera.Priority = 0;
            }
            else
            {
                virtualCamera.Priority = 10;
            }
        }

        isCurrentlyZoomedOut = true;
    }

    public void SwitchBack()
    {
        foreach (CinemachineVirtualCamera virtualCamera in virtualCameras)
        {
            if (virtualCamera != mainCam)
            {
                virtualCamera.Priority = 0;
            }
            else
            {
                virtualCamera.Priority = 10;
            }
        }
        isCurrentlyZoomedOut = false;
    }
}
