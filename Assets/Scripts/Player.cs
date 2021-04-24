using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerMovement playerMovement = null;
    [SerializeField] public PlayerCameraRotation playerCameraRotation = null;
    [SerializeField] public PlayerMakeSphereCreature playerMakeSphereCreature = null;
}