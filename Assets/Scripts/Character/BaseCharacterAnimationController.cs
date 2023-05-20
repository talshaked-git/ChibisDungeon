using UnityEngine;

public abstract class BaseCharacterAnimationController : MonoBehaviour
{
    protected Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public abstract void PlayAnimation(CharacterState state);
}
