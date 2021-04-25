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

    private void FixedUpdate()
    {
        if (Input.GetKey(W) || Input.GetKey(S) || Input.GetKey(A) || Input.GetKey(D))
        {
            UpdateDirections();
        }

        // todo: optimize/refactor

        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;

        if (Input.GetKey(W))
        {
            RotateTo(wDirection);
            transform.localPosition += cameraForward* speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(S))
        {
            RotateTo(sDirection);
            transform.localPosition += -cameraForward * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(A))
        {
            RotateTo(aDirection);
            transform.localPosition += -cameraRight * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(D))
        {
            RotateTo(dDirection);
            transform.localPosition += cameraRight * speed * Time.fixedDeltaTime;
        }
    }

    private void RotateTo(Quaternion targetRotation)
    {
        characterModel.rotation = Quaternion.Lerp(characterModel.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
