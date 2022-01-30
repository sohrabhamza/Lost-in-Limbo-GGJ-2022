using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KillWall : MonoBehaviour
{
    enum KillDevilOrAngel
    {
        devil, angel
    }

    [SerializeField] KillDevilOrAngel killDevilOrAngel;

    private void Start()
    {
        Debug.Log("Kill");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer == 6 && killDevilOrAngel == KillDevilOrAngel.devil) || (other.gameObject.layer == 7 && killDevilOrAngel == KillDevilOrAngel.angel))
        {
            // Destroy(other.gameObject);
            //Play death sound
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            FindObjectOfType<CheckPoint>().OnDead();
        }
    }
    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(transform.position, 5);
    }
}
