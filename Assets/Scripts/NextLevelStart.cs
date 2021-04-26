using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelStart : MonoBehaviour
{
    [SerializeField] Spawner spawnerToActivate = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null || other.GetComponentInParent<Player>() != null)
        {
            spawnerToActivate.Activate();
            gameObject.SetActive(false);
        }
    }
}
