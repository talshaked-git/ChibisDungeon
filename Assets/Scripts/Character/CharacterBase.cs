using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Running,
    Jumping,
    Attacking,
    Hurt,
    Dead
}

public class CharacterBase : MonoBehaviour, ICharacterAnims
{

    public Animator Animator { get; private set; }

    public CharacterState characterState { get; set; }
    private bool isGrounded = false;

    void Awake()
    {
        this.enabled = false;
    }

    public void Init()
    {
        Animator = GetComponent<Animator>();
        characterState = CharacterState.Idle;
    }

    public void PlayAttackAnim()
    {
        throw new System.NotImplementedException();
    }

    public void PlayDeathAnim()
    {
        throw new System.NotImplementedException();
    }

    public void PlayHurtAnim()
    {
        throw new System.NotImplementedException();
    }

    public void PlayIdleAnim()
    {
        Animator.Play("Idle");
    }

    public void PlayJumpAnim()
    {
        if (!isGrounded)
            return;

        Animator.Play("Jump Loop");
    }

    public void PlayMoveAnim()
    {
        Animator.Play("Running");
    }

    public void PlayAnim()
    {
        if (!isGrounded)
            return;

        switch (characterState)
        {
            case CharacterState.Idle:
                PlayIdleAnim();
                break;
            case CharacterState.Running:
                PlayMoveAnim();
                break;
            case CharacterState.Jumping:
                PlayJumpAnim();
                break;
            case CharacterState.Attacking:
                PlayAttackAnim();
                break;
            case CharacterState.Hurt:
                PlayHurtAnim();
                break;
            case CharacterState.Dead:
                PlayDeathAnim();
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platforms")
        {
            isGrounded = true;
            characterState = CharacterState.Idle;
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
            characterState = CharacterState.Jumping;
        }
    }

    public void PlayFallingAnim()
    {
        Animator.Play("Falling Down");
    }
}
