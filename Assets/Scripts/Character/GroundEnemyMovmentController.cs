using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMovmentController : MonoBehaviour, IMovementController
{
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rigidbody2D;
    private BaseCharacterAnimationController animationController;
    private IAttackController attackController;


    private Vector2 velocity;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animationController = GetComponent<BaseCharacterAnimationController>();
        attackController = GetComponent<IAttackController>();
    }

    private void FixedUpdate()
    {
        if (attackController != null && attackController.IsAttacking())
        {
            return;
        }

        // Apply the calculated velocity
        rigidbody2D.velocity = velocity;

        if (velocity.x != 0)
        {
            animationController.PlayAnimation(CharacterState.Running);
        }
        else
        {
            animationController.PlayAnimation(CharacterState.Idle);
        }
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move(float horizontalInput)
    {
        velocity.x = horizontalInput * moveSpeed;

        // Flip the character based on the direction of movement
        Vector3 characterScale = transform.localScale;
        characterScale.x = horizontalInput >= 0 ? Mathf.Abs(characterScale.x) : -Mathf.Abs(characterScale.x);
        transform.localScale = characterScale;
    }
}
