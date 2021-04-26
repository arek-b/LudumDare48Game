using System.Collections;
using UnityEngine;

public class CanMorphIntoSphereCreature : MonoBehaviour
{
    [SerializeField] private Collider myCollider = null;
    [SerializeField] private Rigidbody myRigidbody = null;
    [SerializeField] private CreatureAI creatureAI = null;

    public Collider Collider => myCollider;
    public Rigidbody Rigidbody => myRigidbody;
    public CreatureAI CreatureAI => creatureAI;

    private void OnValidate()
    {
        if (myCollider == null)
            myCollider = GetComponent<Collider>();
        if (myRigidbody == null)
            myRigidbody = GetComponent<Rigidbody>();
        if (creatureAI == null)
            creatureAI = GetComponent<CreatureAI>();
    }

    private SphereCreature sphereCreature = null;

    private void Awake()
    {
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

        if (CreatureAI != null)
        {
            CreatureAI.StopRolling();
            CreatureAI.ResumeRandomNavigation();
        }
    }

    public void SetMorphed(SphereCreature sphereCreature)
    {
        enabled = true;
        this.sphereCreature = sphereCreature;

        if (Collider != null)
            Collider.enabled = false;

        if (Rigidbody != null)
        {
            Rigidbody.useGravity = false;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }

        if (CreatureAI != null)
        {
            CreatureAI.StopAllNavigation();
            CreatureAI.StartRolling();
        }
    }
}