//Volodymyr Pavlik
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private EnemyNavMeshPatrol _enemy;
    private SpawnOnDeath _spawnOnDeath;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnemyNavMeshPatrol>();
        _spawnOnDeath = GetComponentInParent<SpawnOnDeath>();
    }

    public void EndAttack()
    {
        _enemy?.EndAttack();
    }

    public void SpawnChildren()
    {
        if (_spawnOnDeath == null)
        {
            return;
        }
        
        Vector3 deathPosition;

        if (_enemy != null)
        {
            deathPosition = _enemy.transform.position;
        }
        else
        {
            deathPosition = transform.root.position;
        }
        
        Debug.Log(
            $"Enemy died at {_enemy.transform.position}, " +
            $"Spawn component is at {_spawnOnDeath.transform.position}"
        );
        _spawnOnDeath.SpawnAt(deathPosition);
    }

    public void DestroyEnemy()
    {
        GameObject enemyRoot;

        if (_enemy != null)
        {
            enemyRoot = _enemy.gameObject;
        }
        else
        {
            enemyRoot = transform.root.gameObject;
        }
        
        Destroy(enemyRoot);
    }
}