using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetState : IState
{
    private EnemyAI _enemyAI;
    private Transform _target;

    public AutoTargetState(EnemyAI enemyAI)
    {
        _enemyAI = enemyAI;
    }

    public void Enter()
    {
        Debug.Log("Enter AutoTargetState");

        // Assume that the enemy was attacked by the player
        // This can be further improved to consider multiple players
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Execute()
    {
        Debug.Log("Executing AutoTargetState");

        //might want to change this to auto chase even if not in sight
        // If the enemy doesn't see the player, it should return to its previous state
        if (!_enemyAI.IsPlayerInSight())
        {
            _enemyAI.ChangeState(new RoamSearchState(_enemyAI, _enemyAI._patrolRange));
            return;
        }

        // If enemy is within attack range, switch to Attack state
        if (Vector3.Distance(_enemyAI.transform.position, _target.position) <= _enemyAI.attackRange)
        {
            _enemyAI.ChangeState(new AttackState(_enemyAI, _target));
            return;
        }

        // If the player is in sight but not in attack range, the enemy should move towards the player
        Vector3 direction = (_target.position - _enemyAI.transform.position).normalized;
        _enemyAI.SetDestination(_target.position);
        _enemyAI.MoveTowards(Mathf.Sign(direction.x));
    }

    public void Exit()
    {
        Debug.Log("Exit AutoTargetState");
    }
}
