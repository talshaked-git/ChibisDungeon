using UnityEngine;

public class ArcherAnimationController : BaseCharacterAnimationController
{
    public override void PlayAnimation(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Idle:
                animator.CrossFade("Idle",0.1f);
                break;
            case CharacterState.Running:
                animator.Play("Running");
                break;
            case CharacterState.Jumping:
                animator.CrossFade("Jump Loop",0.1f);
                break;
            case CharacterState.Falling:
                animator.CrossFade("Falling Down",0.1f);
                break;
            case CharacterState.Hurt:
                animator.Play("Hurt");
                break;
            case CharacterState.Dead:
                animator.Play("Dying");
                break;
            case CharacterState.Attacking:
                animator.CrossFade("Shooting",0.1f);
                break;
        }
    }
}
