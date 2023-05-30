using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlasherAttack : MonoBehaviour, IAttackController
{
    public Transform swordHitboxPoint;
    public Vector2 hitboxSize = new Vector2(2f, 1f);
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
            StartCoroutine(ResetIsAttacking());
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(attackDamage);
        }
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
