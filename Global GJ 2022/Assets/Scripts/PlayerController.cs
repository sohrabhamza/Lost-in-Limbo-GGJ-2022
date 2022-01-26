using System.Collections;
using System.Collections.Generic;
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

    [Header("Movement Properties")]
    [SerializeField] float groundSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float jumpVelocity;
    [SerializeField] float coyoteTime = 0.35f;
    [SerializeField] float floatFactor = 0.6f;

    // private fields
    private Vector2 movement;
    private bool isGrounded;
    private bool ceilingHit;
    private float gravity = 2 * 9.81f;
    private bool airControl;
    private float coyoteTimeRN = 0.0f;
    private bool readyToJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
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

        if (isEnabled) 
        {
            Movement();
        }

        rb.velocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.x != 0.0f) 
        {
            sprite.flipX = movement.x < 0;
        }

        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    void Movement()
    {
        // floating enabled when F pressed
        airControl = Input.GetKey(KeyCode.F);

        float x = Input.GetAxisRaw("Horizontal");
        movement.x = Mathf.Lerp(movement.x, x * (isGrounded ? groundSpeed : airSpeed), 10.0f * Time.deltaTime);
    
        // Cancels jump when ceiling hit
        if (ceilingHit) 
        {
            movement.y = 0.0f;
        }

        if (isGrounded) 
        {
            readyToJump = true;
            coyoteTimeRN = 0.0f;

            if (Input.GetKey(KeyCode.Space)) 
            {
                animator.SetTrigger("Jump");
                movement.y = jumpVelocity;
            }
            else
            {
                movement.y = 0.0f;
            }
        }
        else
        {
            // Double-jump / mid-air jump
            if (Input.GetKey(KeyCode.Space) && readyToJump && coyoteTime < coyoteTimeRN)
            {
                animator.SetTrigger("Jump");
                movement.y = jumpVelocity;
                readyToJump = false;
            }

            coyoteTimeRN += Time.deltaTime;
            movement.y -= (airControl && movement.y < 0 ? floatFactor : 1.0f) * gravity * Time.deltaTime;
        }
    }
}
