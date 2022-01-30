using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            FindObjectOfType<CheckPoint>().OnDead();
        }
    }
}
