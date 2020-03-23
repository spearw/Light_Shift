using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{

    public void LevelTransition(int i){
        SceneManager.LoadScene(i);
    }

}
