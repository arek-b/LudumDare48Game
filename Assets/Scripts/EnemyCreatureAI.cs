using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCreatureAI : MonoBehaviour
{
    [SerializeField] private float detectCreatureRange = 5f;
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [SerializeField] private Transform destinationsContainer = null;

    private Transform[] destinations;
    private int currentDestination = 0;
    private List<IAttackableByEnemy> attackables = new List<IAttackableByEnemy>();

    private float currentAttackCooldown = 0;

    private float defaultSpeed;
    private float defaultAngularSpeed;
    private float defaultAcceleration;

    private Coroutine patrolCoroutine = null;

    private void Awake()
    {
        destinations = new Transform[destinationsContainer.childCount];
        int i = 0;
        foreach (Transform item in destinationsContainer)
        {
            destinations[i++] = item;
        }

        defaultSpeed = navMeshAgent.speed;
        defaultAngularSpeed = navMeshAgent.angularSpeed;
        defaultAcceleration = navMeshAgent.acceleration;

        patrolCoroutine = StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            navMeshAgent.SetDestination(destinations[currentDestination].position);
            yield return new WaitUntil(() =>
                !navMeshAgent.pathPending &&
                (
                    (navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance <= navMeshAgent.speed / 10f) ||
                    (!navMeshAgent.hasPath)
                )
            );

            currentDestination = NextDestination();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            navMeshAgent.isStopped = true;
        }

        #region AttackCooldown

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        #endregion

        #region CreatureDetection

        Collider[] result = Physics.OverlapSphere(transform.position, detectCreatureRange);
        attackables.Clear();
        Player player = null;

        foreach (var item in result)
        {
            if (player == null)
            {
                player = item.GetComponentInParent<Player>();
                if (player != null)
                {
                    attackables.Add(player);
                    continue;
                }
            }

            CreatureAI creature = item.GetComponent<CreatureAI>();
            if (creature != null)
            {
                attackables.Add(creature);
            }
        }

        IAttackableByEnemy closest = null;
        float closestDistance = float.MaxValue;

        foreach (var item in attackables)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < closestDistance)
            {
                closest = item;
                closestDistance = distance;
            }
        }

        if (closest != null)
        {
            StartCoroutine(Attack(closest));
        }
        #endregion
    }

    private IEnumerator Attack(IAttackableByEnemy target)
    {
        if (currentAttackCooldown > 0)
            yield break;

        currentAttackCooldown = float.MaxValue;

        StopCoroutine(patrolCoroutine);
        Coroutine followCoroutine = StartCoroutine(Follow(target.transform));
        navMeshAgent.speed = defaultSpeed * 3f;
        navMeshAgent.angularSpeed = defaultAngularSpeed * 5f;
        navMeshAgent.acceleration = defaultAcceleration * 10;

        yield return new WaitUntil(() => Vector3.Distance(target.transform.position, transform.position) < 1.5f);
        StopCoroutine(followCoroutine);
        navMeshAgent.ResetPath();

        navMeshAgent.speed = defaultSpeed;
        navMeshAgent.angularSpeed = defaultAngularSpeed;
        navMeshAgent.acceleration = defaultAcceleration;

        currentDestination = FindClosestDestination();
        patrolCoroutine = StartCoroutine(Patrol());

        target.BeAttacked(attackSource: transform);
        currentAttackCooldown = attackCooldown;
    }

    private IEnumerator Follow(Transform target)
    {
        while (true)
        {
            navMeshAgent.SetDestination(target.position);
            yield return null;
        }
    }

    private int NextDestination()
    {
        if (currentDestination >= destinations.Length - 1)
            return 0;
        return currentDestination + 1;
    }

    private int FindClosestDestination()
    {
        int closest = currentDestination;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < destinations.Length; i++)
        {
            float distance = Vector3.Distance(destinations[i].position, transform.position);
            if (distance < closestDistance)
            {
                closest = i;
                closestDistance = distance;
            }
        }

        return closest;
    }
}
