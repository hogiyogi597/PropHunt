using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerMovement : IPlayerMovement
{
    [SerializeField] private float lookSensitivity = 100f;
    [SerializeField] private float lookClampDegrees = 90f;
    private float xRotation = 0f;

    [Header("Look Attributes")]
    public Transform eyes;

    protected override void Update()
    {
        if (IsLocalPlayer)
        {
            Look();
        }
        base.Update();
    }

    void Look()
    {
        Vector2 lookOverTime = inputManager.Look * lookSensitivity * Time.deltaTime;

        xRotation -= lookOverTime.y;
        xRotation = Mathf.Clamp(xRotation, -lookClampDegrees, lookClampDegrees);

        eyes.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookOverTime.x);
    }

    protected override void Move()
    {
        if (inputManager.Movement.magnitude >= 0.1f)
        {
            Vector3 localMovement = transform.right * inputManager.Movement.x + transform.forward * inputManager.Movement.z;
            controller.Move(localMovement * playerSpeed * Time.deltaTime);
        }
    }
}
