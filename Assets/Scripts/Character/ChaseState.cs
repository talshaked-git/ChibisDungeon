using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private EnemyAI _enemyAI;
    Transform target;

    public ChaseState(EnemyAI enemyAI)
    {
        _enemyAI = enemyAI;
    }

    public void Enter()
    {
        Debug.Log("Enter ChaseState");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Execute()
    {
        ChaseTarget();

        // Check if within stop distance
        if (Vector3.Distance(_enemyAI.transform.position, target.position) <= _enemyAI.stopDistance)
        {
            // Change to Attack state
            _enemyAI.ChangeState(new AttackState(_enemyAI, target));
        }

        // Check if the target has moved out of sight
        if (!_enemyAI.IsPlayerInSight())
        {
            // Change back to RoamSearchState
            _enemyAI.ChangeState(new RoamSearchState(_enemyAI, _enemyAI._patrolRange));
        }
    }

    public void Exit()
    {
        Debug.Log("Exit ChaseState");
    }

    private void ChaseTarget()
    {
        Vector3 direction = (target.position - _enemyAI.transform.position).normalized;
        float horizontalInput = Mathf.Sign(direction.x);
        _enemyAI.SetDestination(target.position);
        _enemyAI.MoveTowards( horizontalInput);
    }
}
