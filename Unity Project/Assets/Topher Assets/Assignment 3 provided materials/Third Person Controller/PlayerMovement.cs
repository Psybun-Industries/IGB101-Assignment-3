using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleMovement : MonoBehaviour{

    public Animator anim;
    public float rotSpeed = 10;
    public float walkSpeed = 2;
    public float runSpeed = 4;
    public Rigidbody rb;
    public float jumpForce = 10.0f;
    public bool isGrounded = false;


    // Start is called before the first frame update
    void Start(){    
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        ForwardMovement();
        Turning();
        Jump();
        Actions();
    }

    private void ForwardMovement(){
        if(Input.GetKey("w")){
            anim.SetBool("Walking", true);
            if (Input.GetKey(KeyCode.LeftShift)){
                anim.SetBool("Running", true);
            } else{
                anim.SetBool("Running", false);

            }
        } else if (Input.GetKeyUp("w")) {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }
    }

    private void Turning(){
        if (Input.GetKey("a")) {
            transform.Rotate(0, -rotSpeed * 15 * Time.deltaTime, 0, Space.World);
            anim.SetBool("Turn Left", true);
        } else if (Input.GetKey("d")) {
            transform.Rotate(0, rotSpeed * 15 * Time.deltaTime, 0, Space.World);
            anim.SetBool("Turn Right", true);
        } else {
            anim.SetBool("Turn Left", false);
            anim.SetBool("Turn Right", false);
        }
    }

    private void Actions(){
        if(Input.GetKeyDown("e")){
            anim.SetBool("Waving", true);
        } else if(Input.GetKeyUp("e")){
            anim.SetBool("Waving", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            // Use ForceMode.Impulse for an immediate burst of speed
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("Jumping", true);
        }
        else if (Input.GetKeyUp("space"))
        {
            anim.SetBool("Jumping", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && !anim.GetBool("Jumping"))
        {
            isGrounded = true;
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
            isGrounded = false;
        }
    }
}