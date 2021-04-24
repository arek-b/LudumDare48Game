using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody myRigidbody = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private Transform characterModel = null;

    private const KeyCode W = KeyCode.W;
    private const KeyCode S = KeyCode.S;
    private const KeyCode A = KeyCode.A;
    private const KeyCode D = KeyCode.D;

    private Quaternion wDirection;
    private Quaternion sDirection;
    private Quaternion aDirection;
    private Quaternion dDirection;

    private void UpdateDirections()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        targetRotation *= Quaternion.Euler(0, 180, 0);

        wDirection = targetRotation;
        sDirection = targetRotation * Quaternion.Euler(0, 180, 0);
        aDirection = targetRotation * Quaternion.Euler(0, -90, 0);
        dDirection = targetRotation * Quaternion.Euler(0, 90, 0);
    }

    private void Update()
    {

        if (Input.GetKey(W) || Input.GetKey(S) || Input.GetKey(A) || Input.GetKey(D))
        {
            UpdateDirections();
        }

        if (Input.GetKey(W))
        {
            RotateTo(wDirection);
        }
        if (Input.GetKey(S))
        {
            RotateTo(sDirection);
        }
        if (Input.GetKey(A))
        {
            RotateTo(aDirection);
        }
        if (Input.GetKey(D))
        {
            RotateTo(dDirection);
        }

        if (Input.GetKey(W) || Input.GetKey(S) || Input.GetKey(A) || Input.GetKey(D))
        {
            Vector3 force = -characterModel.forward;
            transform.position += force * speed * Time.deltaTime;
        }
    }

    private void RotateTo(Quaternion targetRotation)
    {
        characterModel.rotation = Quaternion.Lerp(characterModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
