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
        devilAnimator.SetBool("isRunning", x * x + z * z > 0);
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

            if (jumping)    //If jump button pressed
            {
                moveDirection.y = jumpHeight;       //Make the player jump
                devilAnimator.SetTrigger("Jumping");
            }
        }

        devilAnimator.SetBool("isGrounded", grounded);
        devilAnimator.SetFloat("YVelocity", moveDirection.y);

        moveDirection.y -= 10 * Time.deltaTime;     //Gravity

        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;   //Check if grounded

        // transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, ZBounds.x, ZBounds.y));
    }
}
