using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCheckPoint : MonoBehaviour
{
    bool devilEnter, angelEnter;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            devilEnter = true;
        }
        if (other.gameObject.layer == 7)
        {
            angelEnter = true;
        }
    }

    private void Update()
    {
        if (devilEnter && angelEnter)
        {
            FindObjectOfType<CheckPoint>().newSpawnPoint = transform.position;
            Destroy(gameObject);
        }
    }
}
