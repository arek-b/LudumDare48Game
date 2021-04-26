using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Rigidbody myRigidbody = null;
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private Transform characterModel = null;
    [SerializeField] private Collider myCollider = null;
    [SerializeField] private Animator animator = null;

    private const KeyCode W = KeyCode.W;
    private const KeyCode S = KeyCode.S;
    private const KeyCode A = KeyCode.A;
    private const KeyCode D = KeyCode.D;

    private const KeyCode Up = KeyCode.UpArrow;
    private const KeyCode Down = KeyCode.DownArrow;
    private const KeyCode Left = KeyCode.LeftArrow;
    private const KeyCode Right = KeyCode.RightArrow;

    private Quaternion wDirection;
    private Quaternion sDirection;
    private Quaternion aDirection;
    private Quaternion dDirection;

    private float distanceToGround;

    private bool falling = false;

    private void Awake()
    {
        distanceToGround = myCollider.bounds.extents.y;
    }

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
        if (Input.GetKey(W) || Input.GetKey(S) || Input.GetKey(A) || Input.GetKey(D) ||
            Input.GetKey(Up) || Input.GetKey(Down) || Input.GetKey(Left) || Input.GetKey(Right))
        {
            UpdateDirections();
            animator.SetBool(PlayerAnimations.IsRunningBool, true);
        }
        else
        {
            animator.SetBool(PlayerAnimations.IsRunningBool, false);
        }

        // todo: optimize/refactor

        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;

        if (Input.GetKey(W) || Input.GetKey(Up))
        {
            RotateTo(wDirection);
            transform.localPosition += cameraForward* speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(S) || Input.GetKey(Down))
        {
            RotateTo(sDirection);
            transform.localPosition += -cameraForward * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(A) || Input.GetKey(Left))
        {
            RotateTo(aDirection);
            transform.localPosition += -cameraRight * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(D) || Input.GetKey(Right))
        {
            RotateTo(dDirection);
            transform.localPosition += cameraRight * speed * Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            Physics.Raycast(myCollider.bounds.center, -Vector3.up, distanceToGround + 0.1f))
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(PlayerAnimations.JumpTrigger);
        }

        if (myRigidbody.velocity.y < -0.1f &&
            Physics.Raycast(myCollider.bounds.center, -Vector3.up, distanceToGround + 1f))
        {
            StartCoroutine(Falling());
        }
    }

    private IEnumerator Falling()
    {
        if (falling)
            yield break;

        falling = true;
        animator.SetTrigger(PlayerAnimations.JumpLandingTrigger);
        yield return new WaitForSeconds(1f);
        falling = false;
    }

    private void RotateTo(Quaternion targetRotation)
    {
        characterModel.rotation = Quaternion.Lerp(characterModel.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
