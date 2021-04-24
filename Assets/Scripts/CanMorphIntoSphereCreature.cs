using System.Collections;
using UnityEngine;

public class CanMorphIntoSphereCreature : MonoBehaviour
{
    public Collider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ActivatePhysics()
    {
        if (Collider != null)
            Collider.enabled = true;

        if (Rigidbody != null)
            Rigidbody.useGravity = true;
    }

    public void DeactivatePhysics()
    {
        if (Collider != null)
            Collider.enabled = false;

        if (Rigidbody != null)
            Rigidbody.useGravity = false;
    }
}