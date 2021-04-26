﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControlledCreatureManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVcam = null;
    [SerializeField] private CinemachineVirtualCamera sphereCreatureVcam = null;
    [SerializeField] private Player player = null;
    [SerializeField] private SphereCreature sphereCreature = null;

    private bool isSphereCreature = false;

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

    private void LateUpdate()
    {
        if (!isSphereCreature && Input.GetKeyDown(KeyCode.V) && player.playerMakeSphereCreature.MakeSphereCreature())
        {
            SwitchToSphereCreature();
        }
        else if (isSphereCreature && Input.GetKeyDown(KeyCode.V))
        {
            SwitchToPlayer();
        }
    }

    private void SwitchToSphereCreature()
    {
        playerVcam.Priority = int.MinValue;
        sphereCreatureVcam.Priority = int.MaxValue;
        player.playerMovement.enabled = false;
        player.playerCameraRotation.enabled = false;
        player.playerMakeSphereCreature.enabled = false;
        player.gameObject.SetActive(false);
        isSphereCreature = true;
    }

    private void SwitchToPlayer()
    {
        player.transform.position = sphereCreature.transform.position;
        playerVcam.Priority = int.MaxValue;
        sphereCreatureVcam.Priority = int.MinValue;
        sphereCreature.sphereCreatureMovement.enabled = false;
        sphereCreature.playerCameraRotation.enabled = false;
        sphereCreature.ReleaseTransforms();
        player.gameObject.SetActive(true);
        player.playerMovement.enabled = true;
        player.playerCameraRotation.enabled = true;
        player.playerMakeSphereCreature.enabled = true;
        isSphereCreature = false;
    }
}
