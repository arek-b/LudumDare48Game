using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulti : MonoBehaviour
{
    [SerializeField] private PlayerMakeSphereCreature playerMakeSphereCreature = null;
    [SerializeField] private RectTransform ultiFruit = null;
    [SerializeField] private RectTransform key = null;

    private bool hasFruitCached = true;

    private void OnValidate()
    {
        if (playerMakeSphereCreature == null)
            playerMakeSphereCreature = FindObjectOfType<PlayerMakeSphereCreature>();
    }

    private void Update()
    {
        if (hasFruitCached == playerMakeSphereCreature.HasFruit)
            return;

        SetHasFruit(playerMakeSphereCreature.HasFruit);
    }

    private void SetHasFruit(bool hasFruit)
    {
        hasFruitCached = hasFruit;
        ultiFruit.gameObject.SetActive(hasFruit);
        key.gameObject.SetActive(hasFruit);
    }
}
