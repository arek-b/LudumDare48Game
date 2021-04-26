using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObstacleToBreak>())
        {
            other.GetComponent<ObstacleToBreak>().Destruct();
        }

        ControlledCreatureManager.Instance.SwitchToPlayer();
    }
}
