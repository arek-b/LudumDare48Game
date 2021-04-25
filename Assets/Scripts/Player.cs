using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IAttackableByEnemy
{
    [SerializeField] public PlayerMovement playerMovement = null;
    [SerializeField] public PlayerCameraRotation playerCameraRotation = null;
    [SerializeField] public PlayerMakeSphereCreature playerMakeSphereCreature = null;

    public void BeAttacked(Transform attackSource)
    {
        Vector3 direction = -(attackSource.position - transform.position).normalized;
        direction.y += 0.5f;
        transform.GetComponent<Rigidbody>().AddForce(direction * 15f, ForceMode.Impulse);
    }
}