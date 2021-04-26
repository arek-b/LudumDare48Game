using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMakeSphereCreature : MonoBehaviour
{
    [SerializeField] private float searchRadius = 3f;
    [SerializeField] private int minCreatures = 4;
    [SerializeField] private SphereCreature sphereCreature = null;

    private bool hasFruit = false;

    public bool HasFruit => hasFruit;

    public bool MakeSphereCreature()
    {
        if (!hasFruit)
            return false;

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
        hasFruit = false;
        return true;
    }

    private void Update()
    {
        if (hasFruit)
            return;

        Collider[] result = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var item in result)
        {
            SuperpowerFruit superpowerFruit = item.GetComponent<SuperpowerFruit>();
            if (superpowerFruit != null)
            {
                PickUpFruit(superpowerFruit);
                break;
            }
        }
    }

    private void PickUpFruit(SuperpowerFruit fruit)
    {
        fruit.PickUp();
        hasFruit = true;
    }
}
