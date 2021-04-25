using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class TreeAI : MonoBehaviour
{
    [SerializeField] private Transform[] destinations = default;
    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [SerializeField] private Animator animator = null;

    private int currentDestination = 0;
    private float defaultSpeed;

    private const string IdleBool = "Idle";

    private int NextDestination()
    {
        if (currentDestination >= destinations.Length - 1)
            return 0;
        return currentDestination + 1;
    }

    private void Awake()
    {
        defaultSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 0;
    }

    private IEnumerator Start()
    {
        while(true)
        {
            animator.SetBool(IdleBool, false);
            navMeshAgent.SetDestination(destinations[currentDestination].position);
            yield return new WaitUntil(() =>
                !navMeshAgent.pathPending &&
                navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            );
            currentDestination = NextDestination();
            animator.SetBool(IdleBool, true);
            yield return new WaitForSeconds(Mathf.Lerp(5f, 50f, Random.value));
        }
    }

    public void StartMoving()
    {
        navMeshAgent.speed = defaultSpeed;
    }

    public void StopMoving()
    {
        DOTween.To(() => navMeshAgent.speed, v => navMeshAgent.speed = v, 0f, 0.5f);
    }
}
