using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour, IAttackableByEnemy
{
    [SerializeField] private float walkRadius = 20f;
    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [SerializeField] private Animator animator = null;

    private Coroutine walkToRandomPositionsCoroutine = null;
    private Coroutine followCoroutine = null;

    private bool isRolling = false;

    private float totalFollowTime = 0f;

    private bool applicationQuitting = false;

    private const string AnimIsWalkingBool = "IsWalking";
    private const string AnimIsRollingBool = "IsRolling";
    private const string AnimHurtTrigger = "Hurt";
    private const string AnimIsTPoseBool = "IsTPose";

    public delegate void eventCreatureHasDied(CreatureAI creature);
    public static event eventCreatureHasDied CreatureHasDied;

    private void Awake()
    {
        Application.quitting += () => applicationQuitting = true;
        walkToRandomPositionsCoroutine = StartCoroutine(WalkToRandomPositions());
    }

    private void OnDisable()
    {
        if (applicationQuitting)
            return;

        CreatureManager.Instance.UnregisterCreature(this);
    }

    private void Update()
    {
        if (followCoroutine != null)
        {
            if (Vector3.Distance(navMeshAgent.destination, transform.position) < 2.5f)
            {
                navMeshAgent.isStopped = true;
                animator.SetBool(AnimIsWalkingBool, false);
            }
            else
            {
                navMeshAgent.isStopped = false;
                animator.SetBool(AnimIsWalkingBool, true);
            }
        }
    }

    public void StartRolling()
    {
        CreatureManager.Instance.RegisterCreature(this);
        animator.SetBool(AnimIsWalkingBool, false);
        animator.SetBool(AnimIsRollingBool, true);
        isRolling = true;
    }

    public void StopRolling()
    {
        CreatureManager.Instance.UnregisterCreature(this);
        animator.SetBool(AnimIsWalkingBool, false);
        animator.SetBool(AnimIsRollingBool, false);
        animator.SetTrigger(AnimHurtTrigger);
        isRolling = false;
    }

    public void StartFollowing(Transform target)
    {
        if (followCoroutine != null)
            return;

        CreatureManager.Instance.RegisterCreature(this);
        StopAllNavigation();
        navMeshAgent.enabled = true;
        followCoroutine = StartCoroutine(Follow(target));
    }

    private IEnumerator Follow(Transform target)
    {
        animator.SetBool(AnimIsWalkingBool, true);
        float getBoredAfter = Mathf.Lerp(10f, 60f, Random.value);

        //while(totalFollowTime <= getBoredAfter)
        //{
        //    navMeshAgent.SetDestination(target.position);
        //    float waitTime = Mathf.Lerp(0.25f, 0.75f, Random.value);
        //    yield return new WaitForSeconds(waitTime);
        //    totalFollowTime += waitTime;
        //}

        while (true)
        {
            navMeshAgent.SetDestination(target.position);
            float waitTime = Mathf.Lerp(0.25f, 0.75f, Random.value);
            yield return new WaitForSeconds(waitTime);
            totalFollowTime += waitTime;
        }

        //followCoroutine = null;
        //navMeshAgent.isStopped = false;
        //totalFollowTime = 0;
        //animator.SetBool(AnimIsWalkingBool, false);
        //CreatureManager.Instance.UnregisterCreature(this);
        //ResumeRandomNavigation();
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
            followCoroutine = null;
        }

        animator.SetBool(AnimIsWalkingBool, false);
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
        animator.SetBool(AnimIsWalkingBool, false);
        yield return new WaitForSeconds(Mathf.Lerp(0f, 5f, Random.value));
        animator.SetBool(AnimIsWalkingBool, true);
        navMeshAgent.SetDestination(GetRandomPosition());
        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
        );
        animator.SetBool(AnimIsWalkingBool, false);
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
        CreatureHasDied?.Invoke(this);
        StopAllCoroutines();
        enabled = false;
        Destroy(gameObject);
    }
}
