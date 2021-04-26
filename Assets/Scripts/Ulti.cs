using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ulti : MonoBehaviour
{
    [SerializeField] private PlayerMakeSphereCreature playerMakeSphereCreature = null;
    [SerializeField] private RectTransform ultiBack = null;
    [SerializeField] private RectTransform ultiFruit = null;
    [SerializeField] private RectTransform ultiFrame = null;
    [SerializeField] private TextMeshProUGUI textHasFruit = null;
    [SerializeField] private TextMeshProUGUI textNoFruit = null;

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
        textHasFruit.gameObject.SetActive(hasFruit);
        textNoFruit.gameObject.SetActive(!hasFruit);
    }
}
