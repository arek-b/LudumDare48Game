using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCreatureHP : MonoBehaviour
{
    [SerializeField] private int startingHP = 5;
    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private EnemyCreatureAI enemyCreatureAI = null;
    [SerializeField] private SphereCollider myCollider = null;
    
    private int currentHP;

    private bool invincible = false;

    private bool died = false;

    private void OnValidate()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (enemyCreatureAI == null)
            enemyCreatureAI = GetComponent<EnemyCreatureAI>();
        if (myCollider == null)
            myCollider = GetComponentInChildren<SphereCollider>();
    }

    private void Awake()
    {
        currentHP = startingHP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (invincible)
            return;

        if (collision.gameObject.GetComponentInParent<SphereCreature>() == null)
            return;

        if (collision.rigidbody.velocity.magnitude < 2.0f)
            return;

        if (collision.relativeVelocity.magnitude < 2.0f)
            return;

        if (collision.impulse.magnitude < 2.0f)
            return;

        if (currentHP <= 0)
            return;

        currentHP--;

        if (currentHP == 0)
            Die();
        else
            StartCoroutine(TakeDamage());

        ControlledCreatureManager.Instance.SwitchToPlayer();
    }

    private IEnumerator TakeDamage()
    {
        animator.SetTrigger(EnemyCreatureAnimations.HurtTrigger);
        float speed = navMeshAgent.speed;
        float acceleration = navMeshAgent.acceleration;
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = float.MaxValue;
        invincible = true;
        yield return new WaitForSeconds(0.5f);
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;
        invincible = false;
    }

    private void Die()
    {
        if (died)
            return;
        animator.SetTrigger(EnemyCreatureAnimations.DieTrigger);
        enemyCreatureAI.StopAllCoroutines();
        enemyCreatureAI.enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
        myCollider.center += new Vector3(0, -0.5f, -0.75f);
        died = true;
    }
}