using UnityEngine;

public class ArcherAnimationController : BaseCharacterAnimationController
{
    public override bool IsDeadDonePlaying()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isDeadAnimationPlaying = stateInfo.IsName("Dying");  // replace "Dead" with your animation's name
        bool animationFinished = stateInfo.normalizedTime >= 1;
        return animationFinished && isDeadAnimationPlaying;
    }


    public override void PlayAnimation(CharacterState state)
    {
        if (isDead)
            return;

        switch (state)
        {
            case CharacterState.Idle:
                animator.CrossFade("Idle",0.1f);
                break;
            case CharacterState.Walking:
                animator.Play("Walking");
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
                isDead = true;
                break;
            case CharacterState.Attacking:
                animator.CrossFade("Shooting",0.1f);
                break;
        }
    }
}
