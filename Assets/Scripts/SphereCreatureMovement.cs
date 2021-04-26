using System.Collections;
using UnityEngine;

public class SphereCreatureMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public Rigidbody myRigidbody = null;
    [SerializeField] public Camera mainCamera = null;
    [SerializeField] private Collider myCollider = null;

    private const KeyCode W = KeyCode.W;
    private const KeyCode S = KeyCode.S;
    private const KeyCode A = KeyCode.A;
    private const KeyCode D = KeyCode.D;

    private const KeyCode Up = KeyCode.UpArrow;
    private const KeyCode Down = KeyCode.DownArrow;
    private const KeyCode Left = KeyCode.LeftArrow;
    private const KeyCode Right = KeyCode.RightArrow;

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

        if (Input.GetKey(W) || Input.GetKey(Up))
        {
            AddMovement(cameraForward);
        }
        if (Input.GetKey(S) || Input.GetKey(Down))
        {
            AddMovement(-cameraForward);
        }
        if (Input.GetKey(A) || Input.GetKey(Left))
        {
            AddMovement(-cameraRight);
        }
        if (Input.GetKey(D) || Input.GetKey(Right))
        {
            AddMovement(cameraRight);
        }

        if (Input.GetKey(W) || Input.GetKey(Up))
        {
            AddRotation(cameraRight);
        }
        if (Input.GetKey(S) || Input.GetKey(Down))
        {
            AddRotation(-cameraRight);
        }
        if (Input.GetKey(A) || Input.GetKey(Left))
        {
            AddRotation(cameraForward);
        }
        if (Input.GetKey(D) || Input.GetKey(Right))
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