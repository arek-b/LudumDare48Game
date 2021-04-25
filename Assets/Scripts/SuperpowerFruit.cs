using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperpowerFruit : MonoBehaviour
{
    [SerializeField] private float respawnAfter = 60f;
    private Collider myCollider;

    bool active = true;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    public void PickUp()
    {
        StartCoroutine(PickUpCoroutine());
    }

    private IEnumerator PickUpCoroutine()
    {
        MakeActive(false);
        yield return new WaitForSeconds(respawnAfter);
        MakeActive(true);
    }

    private void MakeActive(bool active)
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(active);
        }
        myCollider.enabled = active;
    }
}
