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

    public bool isDead = false;


    [SerializeField] private FloatingHealthBar healthBar;

    [SerializeField] private long expAmount = 100;
    [SerializeField] private int goldDrop = 5;
    [SerializeField] private LootBag lootBag;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        CurrentHp = MaxHp;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.SetMaxValue(MaxHp);
        //lootBag = GetComponent<LootBag>();
    }

    public int GetDamage()
    {
        return Attack;
    }

    public bool IsDead()
    {
        return CurrentHp <= 0;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        int damageAmount = Mathf.Clamp(damage - Defense, 1, int.MaxValue);
        CurrentHp -= damageAmount;
        healthBar.UpdateHealthBar(CurrentHp);
        DamagePopup.Create(transform.position + new Vector3(0, 2.5f), damageAmount);

        if (IsDead())
        {
            Die();
            return;
        }
    }

    private void Die()
    {
        isDead = true;
        enemyAI.ChangeState(new DeadState(enemyAI));
        lootBag.InstantiateLoot(transform.position + Vector3.one);
        ExperienceManager.instance.AddExperience(expAmount);
        GoldManager.instance.AddGold(goldDrop);
    }

    internal void Respawn()
    {
        throw new NotImplementedException();
    }
}
