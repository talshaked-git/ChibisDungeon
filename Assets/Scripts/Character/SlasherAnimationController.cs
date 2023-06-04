using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlasherAnimationController : BaseCharacterAnimationController
{
    public override bool IsDeadDonePlaying()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isDeadAnimationPlaying = stateInfo.IsName("Dying");  // replace "Dead" with your animation's name
        bool animationFinished = stateInfo.normalizedTime >= 1;
        //return animationFinished && isDeadAnimationPlaying;
        return animationFinished;
    }

    public override void PlayAnimation(CharacterState state)
    {
        if (isDead)
            return;

        switch (state)
        {
            case CharacterState.Idle:
                animator.Play("Idle");
                break;
            case CharacterState.Walking:
                animator.Play("Walking");
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
                isDead = true;
                break;
            case CharacterState.Attacking:
                animator.Play("Slashing");
                break;
        }
    }
}