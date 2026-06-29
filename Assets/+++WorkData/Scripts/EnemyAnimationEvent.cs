using System;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private NavMeshEnemy _navMeshEnemy;


    private void Awake()
    {
        _navMeshEnemy = GetComponentInParent<NavMeshEnemy>();
    }

    public void EndAttack()
    {
        _navMeshEnemy.EndAttack();
    }
}