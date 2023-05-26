using System;
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


    [SerializeField] private FloatingHealthBar healthBar;

    [SerializeField] private long expAmount = 100;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        CurrentHp = MaxHp;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetMaxValue(MaxHp);
    }

    public bool IsDead()
    {
        return CurrentHp <= 0;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= Mathf.Clamp(damage - Defense, 1, int.MaxValue);
        healthBar.UpdateHealthBar(CurrentHp);

        if (IsDead())
        {
            Die();
            return;
        }

        if (enemyAI._currentState is RoamSearchState)
        {
            enemyAI.ChangeState(new ChaseState(enemyAI));
        }
    }

    private void Die()
    {
        enemyAI.ChangeState(new DeadState(enemyAI));
        ExperienceManager.instance.AddExperience(expAmount);
    }

    internal void Respawn()
    {
        throw new NotImplementedException();
    }
}
