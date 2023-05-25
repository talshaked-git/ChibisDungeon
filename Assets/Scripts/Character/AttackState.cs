using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private EnemyAI _enemyAI;
    private Transform target;

    public AttackState(EnemyAI enemyAI, Transform target)
    {
        _enemyAI = enemyAI;
        this.target = target;
    }

    public void Enter()
    {
        Debug.Log("Enter AttackState");
    }

    public void Execute()
    {
        // Ensure that we're still within attack range
        if (Vector3.Distance(_enemyAI.transform.position, target.position) <= _enemyAI.attackRange)
        {
            // If we're still in attack range, continue attacking
            AttackTarget();

        }
        else
        {
            // If we're not in attack range, but still close and can see the player, return to ChaseState
            if (_enemyAI.IsPlayerInSight())
            {
                _enemyAI.ChangeState(new ChaseState(_enemyAI));
            }
            else
            {
                // If the player has moved out of sight, return to RoamSearchState
                _enemyAI.ChangeState(new RoamSearchState(_enemyAI, _enemyAI._patrolRange));
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exit AttackState");
    }

    private void AttackTarget()
    {
        // Carry out attack here
        _enemyAI.Attack();
    }
}
