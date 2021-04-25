using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMakeSphereCreature : MonoBehaviour
{
    [SerializeField] private float searchRadius = 3f;
    [SerializeField] private int minCreatures = 4;
    [SerializeField] private SphereCreature sphereCreature = null;

    public bool MakeSphereCreature()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, searchRadius);
        List<CanMorphIntoSphereCreature> morphables = new List<CanMorphIntoSphereCreature>();

        foreach (Collider item in results)
        {
            CanMorphIntoSphereCreature morphable = item.GetComponent<CanMorphIntoSphereCreature>();

            if (morphable != null)
            {
                morphables.Add(morphable);
            }
        }

        if (morphables.Count < minCreatures)
            return false;

        sphereCreature.BeginAssembly(morphables);
        return true;
    }
}
