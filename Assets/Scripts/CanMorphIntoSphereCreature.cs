using System.Collections;
using UnityEngine;

public class CanMorphIntoSphereCreature : MonoBehaviour
{
    public Collider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
}