using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.5f;

    private Vector3 velocityVector;
    private float previousYVelocity;
    private Rigidbody2D rigidbody2D;
    private CharacterBase characterBase;

    void Awake()
    {
        this.enabled = false;
    }

    public void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<CharacterBase>();
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    public void SetCharacterState(CharacterState characterState)
    {
        characterBase.characterState = characterState;
    }

    public void SetVelocityAndState(Vector3 velocityVector, CharacterState characterState)
    {
        this.velocityVector = velocityVector;
        characterBase.characterState = characterState;
    }

    public void AddForce(Vector2 force)
    {
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (characterBase.characterState == CharacterState.Jumping)
        {
            // If jumping, only adjust the x velocity and leave y velocity as is
            rigidbody2D.velocity = new Vector2(velocityVector.x * moveSpeed, rigidbody2D.velocity.y);

            // Check if the y velocity has changed from positive to negative
            if (previousYVelocity > 0f && rigidbody2D.velocity.y < 0f)
            {
                characterBase.PlayFallingAnim();
            }

            previousYVelocity = rigidbody2D.velocity.y;
        }
        else
        {
            // If not jumping, adjust both x and y velocity
            rigidbody2D.velocity = velocityVector * moveSpeed;
        }

        characterBase.PlayAnim();
    }

}
