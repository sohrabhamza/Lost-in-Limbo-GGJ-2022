using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isEnabled = true;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform[] groundChecks;
    [SerializeField] Transform[] ceilingChecks;
    [SerializeField] Light2D point;

    [Header("Movement Properties")]
    [SerializeField] float groundSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpVelocity;
    [SerializeField] float coyoteTime = 0.35f;
    [SerializeField] float floatFactor = 0.6f;
    [SerializeField] float djTime = 0.15f;
    [SerializeField] bool airControl = true;
    [SerializeField] bool allowFloating = true;

    // private fields
    Vector2 movement;
    bool isGrounded;
    bool ceilingHit;
    float gravity = 2 * 9.81f;
    float coyoteTimeRN = 0.0f;
    float djTimer;
    bool readyFirstJump;
    bool readySecondJump;
    float x;
    bool jumping;
    float groundX;
    float pointLightIntensity;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        pointLightIntensity = point.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        PerformAnimation();
        MyInput();
        if (Input.GetKeyDown(KeyCode.X))
        {
            isEnabled = !isEnabled;
        }
        if (!isEnabled)
        {
            point.intensity = Mathf.Lerp(point.intensity, 0, Time.deltaTime * 15);
            return;
        }
        if (x != 0.0f && isEnabled)
        {
            sprite.flipX = x < 0;
        }
        point.intensity = Mathf.Lerp(point.intensity, pointLightIntensity, Time.deltaTime * 15);    //Don't feel like stopping this when its near max; This should be fine. 
    }
    // Called at fixed intervals
    void FixedUpdate()
    {
        // Determines ground and ceiling collisions
        isGrounded = false;
        ceilingHit = false;

        foreach (Transform groundCheck in groundChecks)
        {
            isGrounded |= Physics2D.Raycast(groundCheck.position, Vector2.down, 0.05f);
        }

        foreach (Transform ceilingCheck in ceilingChecks)
        {
            ceilingHit |= Physics2D.Raycast(ceilingCheck.position, Vector2.up, 0.05f);
        }
        Movement();
        rb.velocity = movement;
    }


    void PerformAnimation()
    {
        animator.SetFloat("Horizontal", Mathf.Abs(Mathf.Clamp(rb.velocity.x * 2, -1, 1)));
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    void MyInput()  //Input should be taken in update
    {
        if (isEnabled)
        {
            x = Input.GetAxisRaw("Horizontal");
            jumping = Input.GetKey(KeyCode.Space);
        }
        else
        {
            x = 0;
            jumping = false;
        }
    }

    void Movement()
    {
        bool floating = Input.GetKey(KeyCode.F);

        // Cancels jump when ceiling hit
        if (ceilingHit)
        {
            movement.y = 0.0f;
        }

        if (isGrounded)
        {
            ResetJump();
            coyoteTimeRN = 0.0f;
            djTimer = 0;

            movement.x = Mathf.Lerp(movement.x, x * groundSpeed, 10.0f * Time.deltaTime);

            movement.y = 0.0f;
        }
        else
        {
            // Double-jump / mid-air jump
            if (jumping && djTime <= djTimer && readySecondJump)
            {
                // Debug.Log("Second Jump");
                animator.SetTrigger("Jump");

                groundX = x;
                movement.y = jumpVelocity;
                readySecondJump = false;
            }

            if (airControl)
            { movement.x = Mathf.Lerp(movement.x, x * (Mathf.Sign(groundX) == Mathf.Sign(x) && groundX != 0 && !floating ? groundSpeed : airSpeed), 10.0f * Time.deltaTime); }

            coyoteTimeRN += Time.deltaTime;

            if (!jumping && !readyFirstJump)//Do not allow double jump if first jump is available or if space is being pressed
            {
                djTimer += Time.deltaTime;
            }

            movement.y -= (floating && movement.y < 0 && allowFloating ? floatFactor : 1.0f) * gravity * Time.deltaTime;
        }

        //First jump
        if (jumping && readyFirstJump && coyoteTime >= coyoteTimeRN)
        {
            // Debug.Log("First Jump");
            animator.SetTrigger("Jump");

            groundX = x;
            movement.y = jumpVelocity;
            coyoteTimeRN = coyoteTime;
            readyFirstJump = false;
        }
    }

    void ResetJump()
    {
        if (!jumping)
        {
            readyFirstJump = true;
            readySecondJump = true;
        }
    }
}