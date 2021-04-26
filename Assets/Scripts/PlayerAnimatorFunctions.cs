using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorFunctions : MonoBehaviour
{
    public void DoUseSmash()
    {
        ControlledCreatureManager.Instance.DoUseSmash();
    }
}
