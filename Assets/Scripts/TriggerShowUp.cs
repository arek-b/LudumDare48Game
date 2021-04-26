using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShowUp : MonoBehaviour
{
    [SerializeField] private GameObject _UIPopUp;
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            _UIPopUp.SetActive(true);
        }
    }
}
