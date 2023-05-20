using UnityEngine;

public class ArcherAnimationController : BaseCharacterAnimationController
{
    public override void PlayAnimation(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Idle:
                animator.Play("Idle");
                break;
            case CharacterState.Running:
                animator.Play("Running");
                break;
            case CharacterState.Jumping:
                animator.Play("Jump Loop");
                break;
            case CharacterState.Falling:
                animator.Play("Falling Down");
                break;
            case CharacterState.Hurt:
                animator.Play("Hurt");
                break;
            case CharacterState.Dead:
                animator.Play("Dying");
                break;
            case CharacterState.Attacking:
                animator.Play("Shooting");
                break;
        }
    }
}
