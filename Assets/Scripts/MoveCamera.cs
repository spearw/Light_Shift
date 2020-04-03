using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform target;
    public float dampTime = 0.6f;
    private float xPos = 0.5f;
    public PlayerController playerController;
    private Vector3 velocity = Vector3.zero;
    private bool lookUpPressed;
    private bool lookDownPressed;

    void moveCamera(float yPos){
        Vector3 relativePos = Camera.main.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - Camera.main.ViewportToWorldPoint (new Vector3(xPos, yPos, relativePos.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
    }

    void Start(){
        transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
    }

    void Update(){
        if (Input.GetButtonDown("LookUp")){
            Debug.Log("Button Pressed");
            lookUpPressed = true;
        }
        if (Input.GetButtonUp("LookUp")){
            lookUpPressed = false;
        }
        if (Input.GetButtonDown("LookDown")){
            lookDownPressed = true;
        }
        if (Input.GetButtonUp("LookDown")){
            lookDownPressed = false;
        }
    }
    void LateUpdate()
    {
        if (lookUpPressed){
            moveCamera(0.2f);
        }
        else if (lookDownPressed){
            moveCamera(0.8f);
        }
        else{
            moveCamera(0.4f);
        }
        
    }

}
