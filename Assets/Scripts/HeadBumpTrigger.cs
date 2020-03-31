using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBumpTrigger : MonoBehaviour
{
    public bool HeadBump;
    void OnTriggerStay2D (Collider2D other) {
        if(other.gameObject.CompareTag("Boundary")){
        } else { 
            HeadBump = true;
        }
    }
    void OnTriggerExit2D (Collider2D other) {
        HeadBump = false;
    }
}
