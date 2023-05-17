using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterAnims
{
    void PlayIdleAnim();
    void PlayMoveAnim();
    void PlayJumpAnim();
    void PlayAttackAnim();
    void PlayDeathAnim();
    void PlayHurtAnim();
    void PlayFallingAnim();
}
