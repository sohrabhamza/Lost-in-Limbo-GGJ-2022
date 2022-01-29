using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchToSolo : MonoBehaviour
{
    [SerializeField] Transform player1, player2;
    [SerializeField] float maxDist = 20;
    PlayerController player1C, player2C;
    CinemachineVirtualCamera player1Cam, player2Cam;
    CinemachineVirtualCamera vc;
    CinemachineSwitcher cs;

    private void Start()
    {
        player1C = player1.GetComponent<PlayerController>();
        player2C = player2.GetComponent<PlayerController>();
        player1Cam = player1.GetComponentInChildren<CinemachineVirtualCamera>();
        player2Cam = player2.GetComponentInChildren<CinemachineVirtualCamera>();
        vc = GetComponent<CinemachineVirtualCamera>();
        cs = FindObjectOfType<CinemachineSwitcher>();
    }
    void Update()
    {
        if (!cs.isCurrentlyZoomedOut)
        {
            if (Vector3.Distance(player1.position, player2.position) > maxDist)
            {
                if (player1C.isEnabled)
                {
                    player1Cam.Priority = 5;
                    player2Cam.Priority = 0;
                    vc.Priority = 0;
                }
                else if (player2C.isEnabled)
                {
                    player1Cam.Priority = 0;
                    player2Cam.Priority = 5;
                    vc.Priority = 0;
                }
            }
            else
            {
                player1Cam.Priority = 0;
                player2Cam.Priority = 0;
                vc.Priority = 5;
                // Debug.Log("yes");
            }
        }
        else
        {
            player1Cam.Priority = 0;
            player2Cam.Priority = 0;
            vc.Priority = 0;
        }
    }
}
