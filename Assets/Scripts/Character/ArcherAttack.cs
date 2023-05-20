using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherAttack : MonoBehaviour,IAttackController
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 20f;

    private bool isAttacking = false;
    public float arrowLifetime = 1.8f;
    public float arrowSpawnDelay = 0.3f;
    public float attackCooldown = 1f;
    private float timeSinceLastAttack;

    private BaseCharacterAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<ArcherAnimationController>();
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
            // Calculate the arrow velocity based on the character's facing direction
            float arrowVelocityX = transform.localScale.x * arrowSpeed;
            StartCoroutine(SpawnArrow());
            StartCoroutine(ResetIsAttacking());
        }
    }

    IEnumerator SpawnArrow()
    {
        yield return new WaitForSeconds(arrowSpawnDelay);
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        // Flip the arrow sprite horizontally if the character is facing left
        if (transform.localScale.x < 0)
        {
            arrow.transform.Rotate(new Vector3(0, 180, 0));
            arrow.transform.localPosition += new Vector3(-0.75f, 0, 0);
        }
        else
        {
            arrow.transform.localPosition += new Vector3(0.75f, 0, 0);
        }
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = arrow.transform.right * arrowSpeed;
        Destroy(arrow, arrowLifetime);
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
}
