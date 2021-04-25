using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCallCreatures : MonoBehaviour
{
    [SerializeField] private float radius = 20f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CallCreatures();
        }
    }

    private void CallCreatures()
    {
        Collider[] result = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider item in result)
        {
            CreatureAI creature = item.GetComponent<CreatureAI>();
            if (creature == null)
                continue;

            creature.StartFollowing(transform);
        }
    }
}
