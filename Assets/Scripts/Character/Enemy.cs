using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField]
    private int MaxHp;
    private int CurrentHp;
    [SerializeField]
    private int Attack;
    [SerializeField]
    private int Defense;

    private EnemyAI enemyAI;
    private BaseCharacterAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<BaseCharacterAnimationController>();
        enemyAI = GetComponent<EnemyAI>();
        CurrentHp = MaxHp;
    }

    public bool IsDead()
    {
        return CurrentHp <= 0;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= Mathf.Clamp(damage - Defense, 0, int.MaxValue);
        if(enemyAI._currentState is RoamSearchState)
        {
            enemyAI.ChangeState(new ChaseState(enemyAI));
        }

        if (IsDead())
        {
            animationController.PlayAnimation(CharacterState.Dead);
            enemyAI.ChangeState(new DeadState(enemyAI));
        }
    }
}
