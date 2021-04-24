using System.Collections;
using UnityEngine;

public class SphereCreatureMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody myRigidbody = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private Collider myCollider = null;

    private const KeyCode W = KeyCode.W;
    private const KeyCode S = KeyCode.S;
    private const KeyCode A = KeyCode.A;
    private const KeyCode D = KeyCode.D;

    private Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = myRigidbody.transform.localPosition;
    }

    private void OnEnable()
    {
        myCollider.enabled = true;
        myRigidbody.useGravity = true;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    private void OnDisable()
    {
        myCollider.enabled = false;
        myRigidbody.useGravity = false;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;

        if (Input.GetKey(W))
        {
            AddMovement(cameraForward);
        }
        if (Input.GetKey(S))
        {
            AddMovement(-cameraForward);
        }
        if (Input.GetKey(A))
        {
            AddMovement(-cameraRight);
        }
        if (Input.GetKey(D))
        {
            AddMovement(cameraRight);
        }

        if (Input.GetKey(W))
        {
            AddRotation(cameraRight);
        }
        if (Input.GetKey(S))
        {
            AddRotation(-cameraRight);
        }
        if (Input.GetKey(A))
        {
            AddRotation(cameraForward);
        }
        if (Input.GetKey(D))
        {
            AddRotation(-cameraForward);
        }
    }

    private void FixedUpdate()
    {
        transform.position = myRigidbody.transform.position;
        myRigidbody.transform.localPosition = startingPosition;
    }

    private void AddMovement(Vector3 force)
    {
        myRigidbody.AddForce(force * speed * Time.deltaTime);
    }

    private void AddRotation(Vector3 force)
    {
        myRigidbody.AddTorque(force * rotationSpeed * Time.deltaTime);
    }
}