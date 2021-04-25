using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour, IAttackableByEnemy
{
    [SerializeField] private float walkRadius = 20f;
    [SerializeField] private NavMeshAgent navMeshAgent = null;

    private Coroutine walkToRandomPositionsCoroutine = null;
    private Coroutine followCoroutine = null;

    private float totalFollowTime = 0f;

    private void Awake()
    {
        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    private void Update()
    {
        if (followCoroutine == null)
            return;

        if (Vector3.Distance(navMeshAgent.destination, transform.position) < 2.5f)
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }

    public void StartFollowing(Transform target)
    {
        StopAllNavigation();
        navMeshAgent.enabled = true;
        followCoroutine = StartCoroutine(Follow(target));
    }

    private IEnumerator Follow(Transform target)
    {
        float getBoredAfter = Mathf.Lerp(10f, 60f, Random.value);

        while(totalFollowTime <= getBoredAfter)
        {
            navMeshAgent.SetDestination(target.position);
            float waitTime = Mathf.Lerp(0.25f, 0.75f, Random.value);
            yield return new WaitForSeconds(waitTime);
            totalFollowTime += waitTime;
        }

        followCoroutine = null;
        navMeshAgent.isStopped = false;
        totalFollowTime = 0;
        ResumeRandomNavigation();
    }

    public void StopAllNavigation()
    {
        if (walkToRandomPositionsCoroutine != null)
        {
            StopCoroutine(walkToRandomPositionsCoroutine);
            walkToRandomPositionsCoroutine = null;
        }

        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
            walkToRandomPositionsCoroutine = null;
        }

        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }

    public void ResumeRandomNavigation()
    {
        // this causes errors when far away from the navmesh, can't find a way to fix
        navMeshAgent.enabled = true;

        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    private IEnumerator WalkToRandomPositions()
    {
        yield return new WaitForSeconds(Mathf.Lerp(0f, 5f, Random.value));
        navMeshAgent.SetDestination(GetRandomPosition());
        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
        );
        yield return new WaitForSeconds(Mathf.Lerp(2f, 15f, Random.value));
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

    public void BeAttacked(Transform attackSource)
    {
        StopAllCoroutines();
        enabled = false;
        Destroy(gameObject);
    }
}
