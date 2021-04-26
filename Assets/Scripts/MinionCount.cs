using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinionCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmPro = null;

    private int cachedCount = int.MinValue;

    private void OnValidate()
    {
        if (tmPro == null)
            tmPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (cachedCount != CreatureManager.Instance.Count)
        {
            SetCount(CreatureManager.Instance.Count);
        }
    }

    private void SetCount(int count)
    {
        cachedCount = count;
        tmPro.text = "× " + count;
    }
}
