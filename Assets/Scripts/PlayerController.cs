using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Transform holdPoint;
    public float throwStrength;
    public float throwAngle;

    [Range(10,30)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

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
        Rigidbody2D rbOther = carriedItem.GetComponent<Rigidbody2D>();
        isCarrying = false;
        rbOther.velocity += new Vector2(transform.forward.z * 8, 2 * throwAngle);
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

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
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
        if(Input.GetKeyDown(KeyCode.S) && !isCrouching){
            if(isCarrying && carriedItem){
                dropItem();
            }
            crouch();
        }

        if(Input.GetKeyUp(KeyCode.S) && isCrouching) {
            stand();
        }

        //Grab
        if (Input.GetKeyDown(KeyCode.E)) {

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
