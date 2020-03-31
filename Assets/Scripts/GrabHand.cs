using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHand : MonoBehaviour
{
    public GameObject interactableObject;
    public bool isDropClear;
    void OnTriggerStay2D (Collider2D other) {
        //set this tag to limit what can be picked up
        if (other.gameObject.CompareTag("pickup")){
            interactableObject = other.gameObject;
        }
        else if(other.gameObject.CompareTag("Boundary")){

        }
        else {
            isDropClear = false;
        }
    }
    void OnTriggerEnter2D (Collider2D other){
        isDropClear = false;
    }
    void OnTriggerExit2D (Collider2D other) {
        interactableObject = null;
        isDropClear = true;
    }
}
