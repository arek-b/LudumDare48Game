using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMakeSphereCreature : MonoBehaviour
{
    [SerializeField] private float searchRadius = 3f;
    [SerializeField] private int minCreatures = 4;
    [SerializeField] private SphereCreature sphereCreature = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            MakeSphereCreature();
        }
    }

    private void MakeSphereCreature()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, searchRadius);
        List<Transform> morphableTransforms = new List<Transform>();

        foreach (Collider item in results)
        {
            CanMorphIntoSphereCreature morphable = item.GetComponent<CanMorphIntoSphereCreature>();

            if (morphable != null)
            {
                morphableTransforms.Add(morphable.transform);
                morphable.Collider.enabled = false;
            }
        }

        if (morphableTransforms.Count < minCreatures)
            return;

        sphereCreature.BeginAssembly(morphableTransforms);
    }
}
