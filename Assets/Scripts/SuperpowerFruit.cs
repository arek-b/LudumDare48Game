using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperpowerFruit : MonoBehaviour
{
    private Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    public void PickUp()
    {
        MakeActive(false);
    }

    public void MakeActive(bool active)
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(active);
        }
        myCollider.enabled = active;
    }
}
