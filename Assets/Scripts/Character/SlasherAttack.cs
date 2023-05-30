using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlasherAttack : MonoBehaviour, IAttackController
{
    public Transform swordHitboxPoint;
    public Vector2 hitboxSize = new Vector2(2f, 1f);
    public LayerMask enemyLayers;

    private int attackDamage;
    private bool isAttacking = false;
    public float attackCooldown = 1f;
    private float timeSinceLastAttack;

    private BaseCharacterAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<SlasherAnimationController>();
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    public void Attack()
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            timeSinceLastAttack = 0f;
            isAttacking = true;
            animationController.PlayAnimation(CharacterState.Attacking);
            StartCoroutine(DetectEnemiesHit());
            StartCoroutine(ResetIsAttacking());
        }
    }

    IEnumerator DetectEnemiesHit()
    {
        yield return new WaitForSeconds(0.3f); // Delay for when the attack hits in the animation
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(swordHitboxPoint.position, hitboxSize, 0f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<IDamageable>()?.TakeDamage(attackDamage);
        }
    }



    IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(0.3f); // Adjust this value according to the attack animation duration
        isAttacking = false;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }


    // Draw hitbox in editor
    void OnDrawGizmosSelected()
    {
        if (swordHitboxPoint == null)
            return;

        Gizmos.DrawWireCube(swordHitboxPoint.position, hitboxSize);
    }

    public void SetAttackDamage(int dmg)
    {
        attackDamage = dmg;
    }
}
