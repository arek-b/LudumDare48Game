using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    [SerializeField] private float walkRadius = 20f;
    [SerializeField] private NavMeshAgent navMeshAgent = null;

    private Coroutine walkToRandomPositionsCoroutine = null;

    private void Awake()
    {
        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    public void StopNavigation()
    {
        Debug.Log("StopNavigation");
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        if (walkToRandomPositionsCoroutine != null)
            StopCoroutine(walkToRandomPositionsCoroutine);

    }

    public void ResumeNavigation()
    {
        navMeshAgent.enabled = true;
        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    private IEnumerator WalkToRandomPositions()
    {
        yield return new WaitForSeconds(Mathf.Lerp(1f, 10f, Random.value));
        navMeshAgent.SetDestination(GetRandomPosition());
        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
        );
        yield return new WaitForSeconds(Mathf.Lerp(1f, 10f, Random.value));
        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    private Vector3 GetRandomPosition()
    {
        const int MaxAttempts = 10;
        for (int i = 0; i < MaxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.x *= walkRadius;
            randomDirection.z *= walkRadius;
            Vector3 randomPosition = randomDirection + transform.position;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxDistance: 1.5f, areaMask: 1))
            {
                return hit.position;
            }
        }
        Debug.LogWarning($"Wasn't able to find a valid random position after {MaxAttempts} attempts");
        return transform.position;
    }
}
