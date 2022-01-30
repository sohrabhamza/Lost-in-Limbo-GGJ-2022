using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxReset : MonoBehaviour
{
    Vector3 defPos;
    private void Start()
    {
        defPos = transform.position;
    }

    public void ResetOnDeath()
    {
        transform.position = defPos;
    }
}
