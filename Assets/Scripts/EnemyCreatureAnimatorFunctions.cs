using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreatureAnimatorFunctions : MonoBehaviour
{
    [SerializeField] private EnemyCreatureAI enemyCreatureAI = null;

    private void OnValidate()
    {
        if (enemyCreatureAI == null)
        {
            enemyCreatureAI = GetComponentInParent<EnemyCreatureAI>();
        }
    }

    public void DoAttack()
    {
        enemyCreatureAI.DoAttack();
    }

    public void EndAttack()
    {
        enemyCreatureAI.EndAttack();
    }
}
