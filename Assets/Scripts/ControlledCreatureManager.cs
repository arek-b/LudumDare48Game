using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControlledCreatureManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVcam = null;
    [SerializeField] private CinemachineVirtualCamera sphereCreatureVcam = null;
    [SerializeField] private Player player = null;
    [SerializeField] private SphereCreature sphereCreature = null;

    private static ControlledCreatureManager instance;
    public static ControlledCreatureManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError(nameof(ControlledCreatureManager) + " instance is null");
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SwitchToSphereCreature()
    {
        playerVcam.Priority = int.MinValue;
        sphereCreatureVcam.Priority = int.MaxValue;

        player.playerMovement.enabled = false;
        player.playerCameraRotation.enabled = false;
        player.playerMakeSphereCreature.enabled = false;

        player.gameObject.SetActive(false);

        sphereCreature.sphereCreatureMovement.enabled = true;
        sphereCreature.playerCameraRotation.enabled = true;
    }
}
