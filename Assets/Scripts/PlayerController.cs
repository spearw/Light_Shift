using System.Collections;
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

    [Range(10,30)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float terminalVelocity;

    private bool isCrouching;
    private bool isCarrying;
    public static GameObject interactableObject;
    public static GameObject carriedItem;
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void throwItem(){
        Rigidbody2D rbOther = carriedItem.GetComponent<Rigidbody2D>();
        isCarrying = false;
        rbOther.velocity += new Vector2(transform.forward.z * throwStrength, throwStrength * throwAngle);
        carriedItem = null;
    }
    void dropItem(){
        carriedItem.transform.position = hand.position;
        isCarrying = false;
        carriedItem = null;
    }
    void pickUpItem(){
        isCarrying = true;
        carriedItem = interactableObject;
    }
    void crouch(){
        isCrouching = true;
        gameObject.transform.localScale -= new Vector3(-0.3f, 0.5f, 0f);
        gameObject.transform.localPosition -= new Vector3(0f, 0.5f, 0f);
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2 (0.5f, 2f);
    }
    void stand(){
        isCrouching = false;
        gameObject.transform.localScale += new Vector3(-0.3f, 0.5f, 0f);
        gameObject.transform.localPosition += new Vector3(0f, 0.5f, 0f);
        gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2 (1f, 2f);
    }

    void FixedUpdate()
    {
        moveInput  = Input.GetAxisRaw("Horizontal");
        //moving right
        if ((moveInput > 0)&&(speed < maxSpeed)){
            speed = speed + acceleration * Time.deltaTime;
        } //moving left
        else if ((moveInput < 0)&&(speed > -maxSpeed)){
            speed = speed - acceleration * Time.deltaTime;
        }
        else
        {
            if(speed > deceleration * Time.deltaTime){
                speed = speed - deceleration * Time.deltaTime;
            }
            else if(speed < - deceleration * Time.deltaTime){
                speed = speed + deceleration * Time.deltaTime;
            }
            else{
                speed = 0;
            }
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalVelocity);
    }
    void Update(){

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //Rotate if moving other direction
        if(moveInput > 0){ 
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if(moveInput < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //Jump
        if(isGrounded == true && Input.GetButtonDown("Jump")){
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
        if(rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //Crouch
        if(Input.GetButtonDown("Crouch") && !isCrouching){
            if(isCarrying && carriedItem){
                dropItem();
            }
            crouch();
        }

        if(Input.GetButtonUp("Crouch") && isCrouching) {
            stand();
        }

        //Grab
        if (Input.GetButtonDown("Pickup")) {

            if(!isCarrying && interactableObject && !isCrouching) {
                pickUpItem(); 
            }
            else if(isCarrying && carriedItem) {
                throwItem();
            }
        }

        if (carriedItem != null){
            carriedItem.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            carriedItem.transform.position = holdPoint.position;
        }


    }
    void OnTriggerStay2D (Collider2D other) {
        //set this tag to limit what can be picked up
        if (other.gameObject.CompareTag("pickup")){
            interactableObject = other.gameObject;
        }
    }
    void OnTriggerExit2D (Collider2D other) {
        interactableObject = null;
    }
}
