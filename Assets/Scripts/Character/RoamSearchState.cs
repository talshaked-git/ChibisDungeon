using UnityEngine;

public class RoamSearchState : IState
{
    private EnemyAI _enemyAI;
    private float _patrolRange;
    private Vector3 _pointA, _pointB;
    private bool _movingToB;
    private float moveSpeed;

    public RoamSearchState(EnemyAI enemyAI, float patrolRange)
    {
        _enemyAI = enemyAI;
        _patrolRange = patrolRange;
    }

    public void Enter()
    {
        Debug.Log("Enter RoamSearchState");
  
        GenerateRandomPoints();
        _movingToB = true;
        _enemyAI.SetDestination(_pointB);
        moveSpeed = Random.Range(0.1f, 0.5f);
    }

    public void Execute()
    {
        if (_enemyAI.IsPlayerInSight())
        {
            _enemyAI.ChangeState(new ChaseState(_enemyAI));
        }

        if (_enemyAI.ReachedDestination())
        {
            if (_movingToB)
            {
                _enemyAI.SetDestination(_pointA);

                _enemyAI.MoveTowards(-moveSpeed);
                _movingToB = false;
            }
            else
            {
                _enemyAI.SetDestination(_pointB);
                _enemyAI.MoveTowards(moveSpeed);
                _movingToB = true;
            }
        }
        else
        {
            if (_movingToB)
            {
                _enemyAI.MoveTowards(moveSpeed);
            }
            else
            {
                _enemyAI.MoveTowards(-moveSpeed);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exit RoamSearchState");
    }

    private void GenerateRandomPoints()
    {
        Vector3 randomPoint = _enemyAI.transform.position;
        randomPoint.x -= Random.Range(0, _patrolRange);
        _pointA = randomPoint;
        randomPoint = _enemyAI.transform.position;
        randomPoint.x += Random.Range(0, _patrolRange);
        _pointB = randomPoint;

    }
}
