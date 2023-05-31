using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour, IMovementController
{
    [SerializeField] private float runSpeed = 6.5f;
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;

    private Rigidbody2D rigidbody2D;
    private BaseCharacterAnimationController animationController;

    private Vector2 velocity;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isLookingRight = false;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animationController = GetComponent<BaseCharacterAnimationController>();
    }

    public void Move(float horizontalInput)
    {
        //if(horizontalInput <= 0.5f || horizontalInput >= -0.5f)
        //{
        //    velocity.x = horizontalInput * runSpeed;
        //}
        //else
        //{
        //    velocity.x = horizontalInput * runSpeed;
        //}

        velocity.x = horizontalInput * runSpeed;
        // Flip the character based on the direction of movement
        Vector3 characterScale = transform.localScale;
        if(horizontalInput > 0)
        {
            characterScale.x = Mathf.Abs(characterScale.x);
            isLookingRight = true;
        }
        else if(horizontalInput < 0)
        {
            characterScale.x = -Mathf.Abs(characterScale.x);
            isLookingRight = false;
        }
        else
        {
            if(isLookingRight)
            {
                characterScale.x = Mathf.Abs(characterScale.x);
            }
            else
            {
                characterScale.x = -Mathf.Abs(characterScale.x);
            }
        }
        transform.localScale = characterScale;
    }


    private void FixedUpdate()
    {
        // Handle vertical movement
        if (isJumping && isGrounded)
        {
            velocity.y = jumpForce;
            isJumping = false;
            isGrounded = false;
            animationController.PlayAnimation(CharacterState.Jumping);
        }
        else if (!isGrounded)
        {
            // Apply gravity
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            // If the character is on the ground, we reset the y velocity to zero
            velocity.y = 0;
        }

        // Apply the calculated velocity
        rigidbody2D.velocity = velocity;


        var attackController = GetComponent<IAttackController>();
        if (attackController != null && attackController.IsAttacking())
        {
            return;
        }

        // Determine the character state and play the appropriate animation
        if (!isGrounded)
        {
            if (velocity.y < 0.1f)
            {
                animationController.PlayAnimation(CharacterState.Falling);
            }
        }
        else if (velocity.x != 0)
        {
            animationController.PlayAnimation(CharacterState.Running);
        }
        //else if (velocity.x != 0 && (velocity.x <= 0.5f* runSpeed || velocity.x >= -0.5f* runSpeed))
        //{
        //    animationController.PlayAnimation(CharacterState.Walking);
        //}
        else
        {
            animationController.PlayAnimation(CharacterState.Idle);
        }
    }


    public void Jump()
    {
        // Check if the character is on the ground before allowing the jump
        if (isGrounded)
        {
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
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