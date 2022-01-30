using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform defaultSpawn;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    public Vector3 newSpawnPoint;

    public void OnDead()
    {
        if (newSpawnPoint != Vector3.zero)
        {
            player1.transform.position = newSpawnPoint;
            player2.transform.position = newSpawnPoint + Vector3.right;
        }
        else
        {
            player1.transform.position = defaultSpawn.position;
            player2.transform.position = defaultSpawn.position + Vector3.right;
        }

        BoxReset[] boxResets = FindObjectsOfType<BoxReset>();
        foreach (BoxReset boxReset in boxResets)
        {
            boxReset.ResetOnDeath();
        }
    }
}
