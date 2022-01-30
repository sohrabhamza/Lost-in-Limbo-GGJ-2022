using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AK.Wwise.Event myFootstep;
    public void PlayFootStep()
    {
        myFootstep.Post(gameObject);
        Debug.Log("post");
    }
}
