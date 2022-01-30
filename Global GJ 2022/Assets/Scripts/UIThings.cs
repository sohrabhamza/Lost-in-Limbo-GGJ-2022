using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIThings : MonoBehaviour
{
    [SerializeField] GameObject destroyMe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 || other.gameObject.layer == 7)
        {
            Destroy(destroyMe);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Application.Quit();
        }
    }
}
