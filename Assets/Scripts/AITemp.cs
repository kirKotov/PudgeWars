using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITemp : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints;

    private NavMeshAgent _enemyAgent;

    private void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        PickNewPatrolPoint();
    }

    private void Update()
    {
        PatrolUpdate();
    }

    private void PickNewPatrolPoint()
    {
        _enemyAgent.destination = _patrolPoints[Random.Range(0, _patrolPoints.Count)].position;
    }

    private void PatrolUpdate()
    {
        if (_enemyAgent.remainingDistance == 0)
        {
            PickNewPatrolPoint();
        }
    }
}