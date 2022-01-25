using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public bool isEnabled = true;
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] bool airControl;
    [SerializeField] float airSpeed = 3;

    [Header("Floating")]
    [SerializeField] bool useFloating;
    // [SerializeField] Vector2 ZBounds = new Vector2(-5, 5);
    [SerializeField] float normalGravity = 10;
    [SerializeField] float floatGravity = 2;

    //Serializer Refs
    [Header("References")]
    [SerializeField] SpriteRenderer devilSprite;
    [SerializeField] Light point;
    //Private Refs
    CharacterController controller;
    Animator devilAnimator;

    //Private vars
    float x, z;
    bool grounded;
    Vector3 moveDirection;
    bool jumping;
    bool readyToJump = true;
    bool floating;
    float pointLightIntensity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        devilAnimator = GetComponent<Animator>();
        devilSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        pointLightIntensity = point.intensity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isEnabled = !isEnabled;
        }
        if (!isEnabled)
        {
            point.intensity = Mathf.Lerp(point.intensity, 0, Time.deltaTime * 15);
            return;
        }
        MyInput();
        point.intensity = Mathf.Lerp(point.intensity, pointLightIntensity, Time.deltaTime * 15);    //Don't feel like stopping this when its near max; This should be fine. 
    }

    void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        // z = Input.GetAxisRaw("Vertical");

        jumping = Input.GetKey(KeyCode.Space);

        if (grounded || floating || airControl)
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
        if (isEnabled)
        {
            floating = moveDirection.y < 0 && jumping ? true & useFloating : false;

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
            else if (!grounded && (floating || airControl))
            {
                moveDirection.x = x * airSpeed;
                moveDirection = transform.TransformDirection(moveDirection);    //convert to world space
            }

            devilAnimator.SetBool("isGrounded", grounded);
            devilAnimator.SetFloat("YVelocity", moveDirection.y);
        }
        //If falling and space button held, make character float
        moveDirection.y -= (!grounded && floating ? floatGravity : normalGravity) * Time.deltaTime;     //Gravity

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
