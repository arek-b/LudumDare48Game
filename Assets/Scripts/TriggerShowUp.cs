using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShowUp : MonoBehaviour
{
    [SerializeField] private GameObject _UIPopUp;
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null || other.GetComponentInParent<Player>() != null)
        {
            _UIPopUp.SetActive(true);
            canExit = true;
        }
    }

    private bool canExit = false;

    private void Update()
    {
        if (!canExit)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
