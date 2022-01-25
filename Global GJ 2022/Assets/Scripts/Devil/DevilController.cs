using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] Vector2 ZBounds = new Vector2(-5, 5);
    //Serializer Refs
    [Header("References")]
    [SerializeField] SpriteRenderer devilSprite;
    [SerializeField] Animator devilAnimator;
    //Private Refs
    CharacterController controller;

    //Private vars
    float x, z;
    bool grounded;
    Vector3 moveDirection;
    bool jumping;
    bool readyToJump = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        devilAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MyInput();
        devilSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        // z = Input.GetAxisRaw("Vertical");

        jumping = Input.GetKey(KeyCode.Space);

        if (grounded)
        {
            if (x > 0)
            {
                devilSprite.flipX = false;
            }
            else if (x < 0)
            {
                devilSprite.flipX = true;
            }
        }

        // if velocity squared is greater than zero, trigger running animation
        // devilAnimator.SetBool("isRunning", x * x + z * z > 0);

        //Just set either GetAxisRaw or GetAxis to the blend tree
        devilAnimator.SetFloat("Horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));

    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (grounded)   //If player is grounded
        {
            moveDirection = new Vector3(x * speed, -.75f, /*z * speed*/ 0);   //Set movement vector based on input
            moveDirection = transform.TransformDirection(moveDirection);    //convert to world space

            if (jumping && readyToJump)    //If jump button pressed
            {
                readyToJump = false;
                moveDirection.y = jumpHeight;       //Make the player jump
                devilAnimator.SetTrigger("Jumping");
            }

            StartCoroutine(ResetJump());
        }

        devilAnimator.SetBool("isGrounded", grounded);
        devilAnimator.SetFloat("YVelocity", moveDirection.y);

        //If falling and space button held, make character float
        moveDirection.y -= (!grounded && moveDirection.y < 0 && jumping ? 2 : 10) * Time.deltaTime;     //Gravity

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;   //Check if grounded

        //up down movement removed
        // transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, ZBounds.x, ZBounds.y));

    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.05f);
        if (!jumping && grounded)
        {
            readyToJump = true;
        }
    }
}
