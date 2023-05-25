using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
    private EnemyAI _enemyAI;

    public DeadState(EnemyAI enemyAI)
    {
        _enemyAI = enemyAI;
    }

    public void Enter()
    {
        Debug.Log("Enter DeadState");

        // Play death animation, disable the enemy components or GameObjects
        // Add code here to play the death animation before disabling the enemy.

        // Here, we disable the enemy after it dies.
        _enemyAI.gameObject.SetActive(false);

        // We could also use Destroy() if we don't plan to reuse the enemy object.
        // Destroy(_enemyAI.gameObject);
    }

    public void Execute()
    {
        // In the Dead state, there should be no execution as the enemy is dead.
        // If there's any code here, it might be for a death animation or for cleanup purposes.
    }

    public void Exit()
    {
        // Exiting the DeadState usually doesn't occur as the enemy is dead.
        // But you might need this for respawning or similar functionality.
    }
}
