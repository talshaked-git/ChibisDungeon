using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    public Player player;

    void Start()
    {
        player = new Player();
        player = GameManager.instance.currentPlayer;
        player.SetExpListner();
    }

    public int GetDamage()
    {
        return player.GetDamage();
    }

    private void OnDestroy()
    {
        player.RemoveExpListner();
    }

    public void TakeDamage(int damage)
    {
        int defense = player.GetDefense();
        int damageAmount = Mathf.Clamp(damage - defense, 1, int.MaxValue);
        player.currentHP -= damageAmount;
        //healthBar.UpdateHealthBar(CurrentHp);
        DamagePopup.Create(transform.position + new Vector3(0, 2.5f), damageAmount);

        if (player.currentHP <= 0)
        {
            Die();
            return;
        }
    }

    private void Die()
    {
        //TODO: Die using animation
        Debug.Log("Player is dead");
        GetComponent<BaseCharacterAnimationController>().PlayAnimation(CharacterState.Dead);
    }
}
