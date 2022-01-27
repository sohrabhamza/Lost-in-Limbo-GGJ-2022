using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    public bool isEnabled = true;
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] bool airControl;
    [SerializeField] float airSpeed = 3;
    [SerializeField] float coyoteTime = 0.35f;

    [Header("Floating")]
    [SerializeField] bool useFloating;
    // [SerializeField] Vector2 ZBounds = new Vector2(-5, 5);
    [SerializeField] float normalGravity = 10;
    [SerializeField] float floatGravity = 2;

    //Serializer Refs
    [Header("References")]
    [SerializeField] SpriteRenderer devilSprite;
    [SerializeField] Light point;
    [SerializeField] Transform[] groundChecks;
    [SerializeField] Transform[] ceilingChecks;
    //Private Refs
    // CharacterController controller;
    Rigidbody2D rb;
    Animator devilAnimator;

    //Private vars
    float x, z;
    bool grounded;
    bool ceilingHit;
    Vector2 moveDirection;
    bool jumping;
    bool readyToJump = true;
    bool floating;
    Vector2 groundInput;
    float pointLightIntensity;
    float coyoteTimeRN;

    private void Start()
    {
        // controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
        devilAnimator = GetComponentInChildren<Animator>();
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
            devilAnimator.SetFloat("Horizontal", 0);
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

        //Just set either GetAxisRaw or GetAxis to the blend tree. 
        //Multiplying by 2 to speed up 
        devilAnimator.SetFloat("Horizontal", Mathf.Abs(Mathf.Clamp(Input.GetAxis("Horizontal") * 2, -1, 1)));

    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Movement();
    }

    void Movement()
    {
        if (isEnabled)
        {
            floating = moveDirection.y < 0 && jumping ? true & useFloating : false;

            if (grounded)   //If player is grounded
            {
                moveDirection = new Vector2(x * speed, -.75f);   //Set movement vector based on input
                // moveDirection = transform.TransformDirection(moveDirection);    //convert to world space

                ResetJump();
            }
            else if (!grounded && (floating || airControl))
            {
                moveDirection.x = x * (Mathf.Sign(groundInput.x) == Mathf.Sign(x) && groundInput.x != 0 && !floating ? speed : airSpeed);    //Stop rapid movement direction changes in air
                // moveDirection = transform.TransformDirection(moveDirection);    //convert to world space

                coyoteTimeRN += Time.deltaTime;
            }

            if (jumping && readyToJump && coyoteTimeRN <= coyoteTime)    //If jump button pressed
            {
                readyToJump = false;
                moveDirection.y = jumpHeight;       //Make the player jump
                devilAnimator.SetTrigger("Jump");
                groundInput = new Vector2(x, z);    //Using a vector2 so I don't have to reword everything if we include up down movement again
            }

            if (grounded)
            {
                coyoteTimeRN = 0;
            }
        }
        else if (grounded)  //must still inherit movement if not grounded 
        {
            moveDirection = new Vector2();  //If not done, y component will keep decreasing triggering the fall animation.
            coyoteTimeRN = 0;
        }

        devilAnimator.SetBool("isGrounded", grounded);
        devilAnimator.SetFloat("Vertical", moveDirection.y);
        //If falling and space button held, make character float

        // if (controller.collisionFlags == CollisionFlags.Above) { moveDirection.y = 0; } //Reset Jump if touching a platform above
        if (ceilingHit)
        {
            moveDirection.y = 0;
        }

        moveDirection.y -= (!grounded && floating ? floatGravity : normalGravity) * Time.deltaTime;     //Gravity

        // grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;   //Check if grounded
        rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

        //up down movement removed
        // transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, ZBounds.x, ZBounds.y));

    }

    void CollisionChecks()
    {
        foreach (Transform groundCheck in groundChecks)
        {
            grounded |= Physics2D.Raycast(groundCheck.position, Vector2.down, 0.05f);
        }

        foreach (Transform ceilingCheck in ceilingChecks)
        {
            ceilingHit |= Physics2D.Raycast(ceilingCheck.position, Vector2.up, 0.05f);
        }
    }

    void ResetJump()
    {
        if (!jumping && grounded)
        {
            readyToJump = true;
        }
    }
}
