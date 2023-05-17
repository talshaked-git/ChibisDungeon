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

    void Awake()
    {
        this.enabled = false;
    }
    public void InitComponents()
    {
        myAnimator = GetComponent<Animator>();
        attackButton = GameObject.Find("Attack").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
    }

    void Update()
    {
        myAnimator.SetBool("isAttacking", isAttacking);
        timeSinceLastAttack += Time.deltaTime;
    }

    public void Attack()
    {
        // if (timeSinceLastAttack >= attackCooldown)
        // {
        //     timeSinceLastAttack = 0f;
        //     isAttacking = true;
        //     StartCoroutine(SpawnArrow());
        //     StartCoroutine(ResetIsAttacking());
        // }
        if (timeSinceLastAttack >= attackCooldown)
        {
            timeSinceLastAttack = 0f;
            isAttacking = true;
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
        yield return new WaitForSeconds(0.5f); // Adjust this value according to the attack animation duration
        isAttacking = false;
    }
}
