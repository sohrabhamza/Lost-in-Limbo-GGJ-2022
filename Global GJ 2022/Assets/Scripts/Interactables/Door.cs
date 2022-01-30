using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float openDistance;
    [SerializeField] float openSpeed;
    Vector2 defaultDoorPosition;
    enum OpenDirection
    {
        x, y
    }
    [SerializeField] OpenDirection openDirection = OpenDirection.y;
    bool state;

    private void Start()
    {
        defaultDoorPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, defaultDoorPosition.x + (state && openDirection == OpenDirection.x ? openDistance : 0), Time.deltaTime * openSpeed),
        Mathf.Lerp(transform.localPosition.y, defaultDoorPosition.y + (state && openDirection == OpenDirection.y ? openDistance : 0), Time.deltaTime * openSpeed));
    }

    public void ChangeState()
    {
        state = !state;
    }
}
