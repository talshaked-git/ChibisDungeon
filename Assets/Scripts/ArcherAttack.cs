using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 20f;
    private Animator myAnimator;
    public Button attackButton;
    private bool isAttacking = false;
    public float arrowLifetime = 1.8f;
    public float arrowSpawnDelay = 0.3f;
    public float attackCooldown = 1f;
    private float timeSinceLastAttack;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        attackButton.onClick.AddListener(Attack);
    }

    void Update()
    {
        myAnimator.SetBool("isAttacking", isAttacking);
        timeSinceLastAttack += Time.deltaTime;
    }

    public void Attack()
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            timeSinceLastAttack = 0f;
            isAttacking = true;
            StartCoroutine(SpawnArrow());
            StartCoroutine(ResetIsAttacking());
        }
    }

    IEnumerator SpawnArrow()
    {
        yield return new WaitForSeconds(arrowSpawnDelay);
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * arrowSpeed, 0);
        Destroy(arrow, arrowLifetime);
    }

    IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this value according to the attack animation duration
        isAttacking = false;
    }
}
