using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float openDistance;
    [SerializeField] float openSpeed;
    Vector2 defaultDoorPosition;
    bool state;

    private void Start()
    {
        defaultDoorPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultDoorPosition.y + (state ? openDistance : 0), Time.deltaTime * openSpeed));
    }

    public void ChangeState()
    {
        state = !state;
    }
}
