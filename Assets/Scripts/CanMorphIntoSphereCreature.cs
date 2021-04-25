using System.Collections;
using UnityEngine;

public class CanMorphIntoSphereCreature : MonoBehaviour
{
    public Collider Collider { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private SphereCreature sphereCreature = null;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        enabled = false;
    }

    private void FixedUpdate()
    {
        Rigidbody sphereRigidbody = sphereCreature.sphereCreatureMovement.myRigidbody;

        if (sphereRigidbody.velocity == Vector3.zero)
            return;

        Quaternion lookRotationVelocity = Quaternion.LookRotation(sphereRigidbody.velocity);

        Quaternion lookRotation = Quaternion.Euler
        (
            0,
            lookRotationVelocity.eulerAngles.y,
            0
        );
        transform.rotation = lookRotation;
    }

    public void SetNotMorphed()
    {
        enabled = false;
        sphereCreature = null;

        if (Collider != null)
            Collider.enabled = true;

        if (Rigidbody != null)
            Rigidbody.useGravity = true;
    }

    public void SetMorphed(SphereCreature sphereCreature)
    {
        enabled = true;
        this.sphereCreature = sphereCreature;

        if (Collider != null)
            Collider.enabled = false;

        if (Rigidbody != null)
            Rigidbody.useGravity = false;
    }
}