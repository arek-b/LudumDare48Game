using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleToBreak : MonoBehaviour
{
    [SerializeField] private GameObject _animatedObject;
    public void Destruct()
    {
        _animatedObject.GetComponent<Animator>().enabled = true;
    }
}
