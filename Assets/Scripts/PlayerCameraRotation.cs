using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRotation : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform cameraFollowPoint = null;

    const float Min = -30;
    const float Max = 85;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 euler = cameraFollowPoint.eulerAngles;
        euler.y += mouseX * speed * Time.deltaTime;
        if (euler.x > 180)
        {
            euler.x -= 360;
        }
        euler.x = Mathf.Clamp(euler.x + (-mouseY * speed * Time.deltaTime), Min, Max);
        cameraFollowPoint.rotation = Quaternion.Euler(euler);
    }
}
