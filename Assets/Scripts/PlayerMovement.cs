using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public MoveJoystick movementJoystick;
    public float playerSpeed = 5f;
    public float playerJump = 1500f;
    private Rigidbody2D rb;
    private Animator myAnimator;
    public bool isGrounded;
    CapsuleCollider2D myCapsuleCollider;
    private bool jumpPressed = false;
    public string jumpButtonName = "Jump";
    public Button jumpButton;


    public void InitComponents()
    {
        jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        jumpButton.onClick.AddListener(Jump);
        movementJoystick = GameObject.Find("JoystickArea").GetComponent<MoveJoystick>();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
{
    if (isGrounded && Mathf.Abs(rb.velocity.y) < 0.01f)
    {
        if (Input.GetButtonDown(jumpButtonName))
        {
            jumpPressed = true;
        }
    }
}


    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        float horizontalMove = movementJoystick.joystickVec.x;

        if (Mathf.Abs(horizontalMove) > 0)
        {
            rb.velocity = new Vector2(horizontalMove * playerSpeed, rb.velocity.y);
            myAnimator.SetBool("isRunning", true);

            // Flip the character based on the direction of movement
            Vector3 characterScale = transform.localScale;
            characterScale.x = horizontalMove > 0 ? Mathf.Abs(characterScale.x) : -Mathf.Abs(characterScale.x);
            transform.localScale = characterScale;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            myAnimator.SetBool("isRunning", false);
        }

        if (jumpPressed)
        {
            Jump();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = true;
            myAnimator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = false;
        }
    }

    // public void Jump()
    // {
    //     rb.velocity = new Vector2(rb.velocity.x, playerJump);
    //     jumpPressed = false;
    //     isGrounded = false;
    //     myAnimator.SetBool("isJumping", true);
    // }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJump);
            isGrounded = false;
            myAnimator.SetBool("isJumping", true);
        }
    }


}
