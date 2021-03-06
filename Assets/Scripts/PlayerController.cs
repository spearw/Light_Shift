﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 0;
    [Range(5, 15)]
    public float maxSpeed = 10;
    public float acceleration = 10;
    public float deceleration = 10; 
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Transform holdPoint;
    public Transform hand;
    public float throwStrength;
    public float throwAngle;

    private bool jumpButtonPressed;
    private bool startJump;
    [Range(10,30)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float terminalVelocity;

    private HeadBumpTrigger headBumpTrigger;
    private bool isCrouching;

    private bool pickupButtonPressed;
    private GrabHand grabHand;
    private bool isCarrying;
    private RigidbodyConstraints2D previousConstraints;
    public static GameObject carriedItem;
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        headBumpTrigger =  gameObject.GetComponentInChildren<HeadBumpTrigger>();
        grabHand =  gameObject.GetComponentInChildren<GrabHand>();
    }

    void throwItem(){
        Rigidbody2D rbOther = carriedItem.GetComponent<Rigidbody2D>();
        isCarrying = false;
        carriedItem.GetComponent<Rigidbody2D>().constraints = previousConstraints;
        rbOther.velocity += new Vector2(transform.forward.z * throwStrength, throwStrength * throwAngle);
        carriedItem = null;
    }
    void dropItem(){
        if (grabHand.isDropClear){
            carriedItem.transform.position = hand.position;
        }
        else {
            carriedItem.GetComponent<Rigidbody2D>().velocity += new Vector2(0, throwStrength * 0.4f);
        }
        carriedItem.GetComponent<Rigidbody2D>().constraints = previousConstraints;
        isCarrying = false;
        carriedItem = null;
    }
    void pickUpItem(){
        isCarrying = true;
        carriedItem = grabHand.interactableObject;
        carriedItem.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        previousConstraints = carriedItem.GetComponent<Rigidbody2D>().constraints;
        carriedItem.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        carriedItem.transform.rotation = new Quaternion(0, 0, 0, 0);
        carriedItem.GetComponent<Rigidbody2D>().position = holdPoint.transform.position;
    }
    void crouch(){
        isCrouching = true;
        gameObject.transform.localScale -= new Vector3(-0.3f, 0.5f, 0f);
        gameObject.transform.localPosition -= new Vector3(0f, 0.5f, 0f);
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2 (0.5f, 2f);
    }
    void stand(){
        if (headBumpTrigger.HeadBump){

        }else {
            isCrouching = false;
            gameObject.transform.localScale += new Vector3(-0.3f, 0.5f, 0f);
            gameObject.transform.localPosition += new Vector3(0f, 0.5f, 0f);
            gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2 (1f, 2f);
        }
    }

    void moveCarriedItem(){
        var carriedItemRB = carriedItem.GetComponent<Rigidbody2D>();
        var holdPoint2D = new Vector2(holdPoint.position.x, holdPoint.position.y);

        carriedItemRB.MovePosition(holdPoint2D);

        Vector2 delta = carriedItemRB.position - holdPoint2D;
        Debug.Log("delta: " + delta);

        if (delta.magnitude > 0.3){
            carriedItemRB.constraints = RigidbodyConstraints2D.None;
            isCarrying = false;
            carriedItem = null;
        }
    }

    void FixedUpdate()
    {
        //movement

        //Rotate if moving other direction
        if(moveInput > 0){ 
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if(moveInput < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //moving right
        if ((moveInput > 0)&&(speed < maxSpeed * moveInput)){
            speed = speed + acceleration * Time.deltaTime;
        } //moving left
        else if ((moveInput < 0)&&(speed > -maxSpeed * -moveInput)){
            speed = speed + -acceleration * Time.deltaTime;
        }
        else if((moveInput > 0) && (speed == maxSpeed)){
            //leave speed
        }
        else if((moveInput < 0) && (speed == -maxSpeed)){
            //leave speed
        }
        else
        {
            if(speed > deceleration * Time.deltaTime){
                speed = speed + -deceleration * Time.deltaTime;
            }
            else if(speed < -deceleration * Time.deltaTime){
                speed = speed + deceleration * Time.deltaTime;
            }
            else{
                speed = 0;
            }
        }
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(speed, rb.velocity.y);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalVelocity);

        //jumping
        if(startJump & isGrounded){
            if (isCrouching && headBumpTrigger.HeadBump == false){
                stand();
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
                startJump = false;
            }
            else if (isCrouching == false){
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
                startJump = false;
            }
            else{
                //can't jump
            }
        }
        if(rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !jumpButtonPressed){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //move carried item
        if (carriedItem != null){
            moveCarriedItem();
        }
    }
    void Update(){

        moveInput  = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (Input.GetButtonDown("Jump")){
            jumpButtonPressed = true;
        }
        if (Input.GetButtonDown("Jump") && isGrounded){
            startJump = true;
        }
        if (Input.GetButtonUp("Jump")){
            jumpButtonPressed = false;
            startJump = false;
        }

        if(Input.GetButtonDown("Crouch")) {

            if (isCrouching){
                stand();
            }
            else{
                if(isCarrying && carriedItem){
                    dropItem();
                }
                else{
                    crouch();
                }
            }
        }
        //Grab
        if (Input.GetButtonDown("Pickup")) {

            if(!isCarrying && grabHand.interactableObject && !isCrouching) {
                pickUpItem(); 
            }
            else if(!isCarrying && grabHand.interactableObject && !headBumpTrigger.HeadBump){
                stand();
                pickUpItem();
            }
            else if(isCarrying && carriedItem) {
                throwItem();
            }
        }
    }
}
