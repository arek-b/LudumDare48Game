using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rigidbodies;

    bool triggered = false;

    private void Update()
    {
        if (triggered)
            return;

        Collider[] result = Physics.OverlapSphere(transform.position, 1.2f);

        foreach (var item in result)
        {
            Player player = item.GetComponentInParent<Player>();
            if (player != null)
            {
                foreach (var rb in rigidbodies)
                {
                    rb.isKinematic = false;
                }
                triggered = true;
                return;
            }
        }
    }
}
