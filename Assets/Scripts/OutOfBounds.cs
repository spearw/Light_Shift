using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBounds : MonoBehaviour
{

    public GameObject grabObject;
    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Restarting Level");
        if (other.gameObject.CompareTag("Player")){
            grabObject = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
