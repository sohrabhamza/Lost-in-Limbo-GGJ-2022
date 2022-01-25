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
    Vector2 groundInput;
    float pointLightIntensity;
    float coyoteTimeRN;

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

        //Just set either GetAxisRaw or GetAxis to the blend tree. 
        //Multiplying by 2 to speed up 
        devilAnimator.SetFloat("Horizontal", Mathf.Abs(Mathf.Clamp(Input.GetAxis("Horizontal") * 2, -1, 1)));

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

                groundInput = new Vector2(x, z);    //Using a vector2 so I don't have to reword everything if we include up down movement again
                StartCoroutine(ResetJump());
            }
            else if (!grounded && (floating || airControl))
            {
                moveDirection.x = x * (Mathf.Sign(groundInput.x) == Mathf.Sign(x) && groundInput.x != 0 ? speed : airSpeed);    //Stop rapid movement direction changes in air
                moveDirection = transform.TransformDirection(moveDirection);    //convert to world space

                coyoteTimeRN += Time.deltaTime;
            }

            if (jumping && readyToJump && coyoteTimeRN <= coyoteTime)    //If jump button pressed
            {
                readyToJump = false;
                moveDirection.y = jumpHeight;       //Make the player jump
                devilAnimator.SetTrigger("Jumping");
            }

            if (grounded)
            {
                coyoteTimeRN = 0;
            }
        }
        else if (grounded)  //must still inherit movement if not grounded 
        {
            moveDirection = new Vector3();  //If not done, y component will keep decreasing triggering the fall animation.
            coyoteTimeRN = 0;
        }

        devilAnimator.SetBool("isGrounded", grounded);
        devilAnimator.SetFloat("YVelocity", moveDirection.y);
        //If falling and space button held, make character float

        if (controller.collisionFlags == CollisionFlags.Above) { moveDirection.y = 0; } //Reset Jump if touching a platform above

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
