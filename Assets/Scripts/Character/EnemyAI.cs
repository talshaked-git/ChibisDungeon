using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;
    public IState _currentState;

    //public Transform target;
    public float targetUpdateRate = 0.5f;
    public float pathUpdateRate = 2f;
    public float _patrolRange = 1.5f;
    public float sightRange = 2.5f;
    public float stopDistance = 2f; // distance at which enemy stops moving towards the target
    public float attackRange = 2f; // distance at which enemy starts attacking

    private Vector3 _destination;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 3f;

    private IMovementController movementController;
    private IAttackController attackController;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        seeker = GetComponent<Seeker>();
        movementController = GetComponent<IMovementController>();
        attackController = GetComponent<IAttackController>();

        _currentState = new RoamSearchState(this, _patrolRange);
        _currentState.Enter();

        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
        seeker.StartPath(transform.position, _destination, OnPathComplete);
    }



    private void Update()
    {
        _currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public bool ReachedDestination()
    {
        //_destination = destination;
        float reachThreshold = 0.5f;
        float distance = Vector3.Distance(transform.position, _destination);
        return distance <= reachThreshold;
    }

    public bool IsPlayerInSight()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //return distanceToPlayer <= sightRange && Vector3.Dot(transform.forward, directionToPlayer) > 0;
        return distanceToPlayer <= sightRange;
    }

    public void MoveTowards( float direction)
    {

        if (path != null && currentWaypoint < path.vectorPath.Count)
        {
            Vector2 directionToWaypoint = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;

            if (Mathf.Sign(directionToWaypoint.x) == Mathf.Sign(direction))
            {
                movementController.Move(direction);
            }
            else
            {
                movementController.Move(0);
            }

            if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, _destination, OnPathComplete);
        }
    }

    public void Attack()
    {
        movementController.Move(0);
        attackController.Attack();
    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Path received. Error: " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    internal void SetDestination(Vector3 pointB)
    {
        _destination = pointB;
    }
}
