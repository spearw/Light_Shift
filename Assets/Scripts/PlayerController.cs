using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float speed;
    public float jumpForce;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Transform holdPoint;
    public float throwStrength;
    public float throwAngle;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool isCrouching;
    private bool isCarrying;
    public static GameObject interactableObject;
    public static GameObject carriedItem;
    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        Debug.Log("Starting...");
    }

    void throwItem(){
        Rigidbody2D rb = carriedItem.GetComponent<Rigidbody2D>();
        isCarrying = false;
        rb.velocity += new Vector2(transform.forward.z * throwStrength, throwStrength * throwAngle);
        rb.isKinematic = false;
        carriedItem = null;
    }
    void dropItem(){
        Rigidbody2D rb = carriedItem.GetComponent<Rigidbody2D>();
        isCarrying = false;
        rb.velocity += new Vector2(transform.forward.z * 8, 2 * throwAngle);
        rb.isKinematic = false;
        carriedItem = null;
    }
    void pickUpItem(){
        isCarrying = true;
        Debug.Log("Picking up" + interactableObject);
        carriedItem = interactableObject;
        float offset = 1f + carriedItem.transform.localScale.y * 0.5f;
        carriedItem.GetComponent<Rigidbody2D>().isKinematic = true;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(moveInput * speed, rigidBody.velocity.y);
    }
    void Update(){

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //Rotate if moving other direction
        if(moveInput > 0){ 
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if(moveInput < 0){
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space)){
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigidBody.velocity = Vector2.up * jumpForce;
        }

        if(Input.GetKey(KeyCode.Space) && isJumping){
            if(jumpTimeCounter > 0){
                rigidBody.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else {
                isJumping = false;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Space)){
            isJumping = false;
        }

        if(Input.GetKeyDown(KeyCode.S) && !isCrouching){
            if(isCarrying && carriedItem){
                dropItem();
            }
            crouch();
        }

        if(Input.GetKeyUp(KeyCode.S) && isCrouching) {
            stand();
        }

        if (Input.GetKeyDown(KeyCode.E)) {

            if(!isCarrying && interactableObject && !isCrouching) {
                pickUpItem(); 
            }
            else if(isCarrying && carriedItem) {
                throwItem();
            }
        }

        if (carriedItem != null){
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
