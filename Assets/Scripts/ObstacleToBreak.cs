using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleToBreak : MonoBehaviour
{
    [SerializeField] private GameObject _animatedObject;
    [SerializeField] private bool _ifExploding = false;
    [SerializeField] private GameObject _exploPrefab;
    [SerializeField] private Transform _placeToExplode;
    public void Destruct()
    {
        _animatedObject.GetComponent<Animator>().enabled = true;
        if (_ifExploding == true)
        {
            GameObject loot = Instantiate(_exploPrefab, transform.position, Quaternion.identity) as GameObject;
            loot.transform.position = _placeToExplode.transform.position;
        }
    }
}
