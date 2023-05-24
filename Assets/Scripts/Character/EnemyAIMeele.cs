using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIMeele : MonoBehaviour
{
    public Transform target;
    public float targetUpdateRate = 0.5f;
    public float pathUpdateRate = 2f;
    public float scoutRange = 8f;
    public float stopDistance = 2f; // distance at which enemy stops moving towards the target
    public float attackRange = 2f; // distance at which enemy starts attacking

    private Vector3 originalPosition; // for storing the original position
    private bool isReturning = false;

    Path path;
    int currentWaypoint = 0;
    public float nextWaypointDistance = 3f;
    bool reachedEndOfPath = false;

    Seeker seeker;

    private IMovementController movementController;
    private IAttackController attackController;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        movementController = GetComponent<IMovementController>();
        attackController = GetComponent<IAttackController>();

        originalPosition = transform.position; // Save the original position

        InvokeRepeating("UpdateTarget", 0f, targetUpdateRate);
        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
    }

    void UpdateTarget()
    {
        Transform enemy = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToTarget = Vector3.Distance(transform.position, enemy.position);
        if (distanceToTarget <= scoutRange)
        {
            target = enemy;
            isReturning = false;
        }
        else
        {
            target = null;
            if (!isReturning)
            {
                isReturning = true;
                seeker.StartPath(transform.position, originalPosition, OnPathComplete);
            }
        }
    }

    void UpdatePath()
    {
        if (target == null)
        {
            return;
        }

        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    private void Update()
    {

        // When returning, use the original position as the target.
        if (isReturning)
        {
            if (Vector2.Distance(transform.position, originalPosition) > nextWaypointDistance)
            {
                Vector2 moveDirection = ((Vector2)originalPosition - (Vector2)transform.position).normalized;
                movementController.Move(moveDirection.x);
            }
            else
            {
                isReturning = false;
                movementController.Move(0);
            }
        }

        if (target == null)
        {
            return;
        }

        if (path == null)
            return;
        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
        movementController.Move(direction.x);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


        // Check if within stop distance
        if (Vector3.Distance(transform.position, target.position) <= stopDistance)
        {
            movementController.Move(0); // Assume you have a method to stop the enemy

            // Check if within attack range
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                attackController.Attack(); // Assume you have a method to attack the target
            }
        }
        else
        {
            movementController.Move(direction.x);
        }

    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Path found. Error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

}
