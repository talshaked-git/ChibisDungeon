using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;


public class PlayerMovmentJoystick : MonoBehaviour
{
    [SerializeField] private float jumpForce = 30f;

    public MoveJoystick movementJoystick;
    private Rigidbody2D rb;
    public bool isGrounded = false;
    CapsuleCollider2D myCapsuleCollider;
    private bool jumpPressed = false;
    public string jumpButtonName = "Jump";
    public Button jumpButton;
    private CharacterBase characterBase;

    void Awake()
    {
        this.enabled = false;
    }

    public void Init()
    {
        jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        jumpButton.onClick.AddListener(Jump);
        movementJoystick = GameObject.Find("JoystickArea").GetComponent<MoveJoystick>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        characterBase = GetComponent<CharacterBase>();

        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        float horizontalMove = movementJoystick.joystickVec.x;

        if(Mathf.Abs(horizontalMove) > 0)
        {
            // Flip the character based on the direction of movement
            Vector3 characterScale = transform.localScale;
            characterScale.x = horizontalMove > 0 ? Mathf.Abs(characterScale.x) : -Mathf.Abs(characterScale.x);
            transform.localScale = characterScale;

            GetComponent<MoveVelocity>().SetVelocityAndState(new Vector2(horizontalMove,0),CharacterState.Running);
        }
        else
        {
            GetComponent<MoveVelocity>().SetVelocity(new Vector2(0, rb.velocity.y));
            if(isGrounded)
            {
                GetComponent<MoveVelocity>().SetCharacterState(CharacterState.Idle);
            }
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            GetComponent<MoveVelocity>().AddForce(new Vector2(0f, jumpForce));
            GetComponent<MoveVelocity>().SetCharacterState(CharacterState.Jumping);
            isGrounded = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = false;
        }
    }
}
