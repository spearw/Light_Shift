using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform target;
    public float dampTime = 0.6f;
    public PlayerController playerController;
    private Vector3 velocity;

    void Awake(){
        velocity = new Vector3(playerController.maxSpeed, playerController.terminalVelocity, 0f);
    }
    void LateUpdate()
    {
        Vector3 relativePos = Camera.main.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, relativePos.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
        
    }
}
