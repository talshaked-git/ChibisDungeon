using UnityEngine;

public abstract class BaseCharacterAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected bool isDead = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void PlayAnimation(CharacterState state);

    public abstract bool IsDeadDonePlaying();
}
