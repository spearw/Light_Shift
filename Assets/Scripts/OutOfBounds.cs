using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBounds : MonoBehaviour
{
    void Awake(){


    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (other.gameObject.CompareTag("pickup") || other.gameObject.CompareTag("nopickup")){
            other.transform.position = other.transform.parent.position;
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

}
