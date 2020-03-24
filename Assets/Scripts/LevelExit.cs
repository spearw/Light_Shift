using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
