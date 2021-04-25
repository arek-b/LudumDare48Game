using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnValidate()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        Vector3 rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).eulerAngles;
        rotation.x = transform.rotation.eulerAngles.x;
        rotation.z = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
