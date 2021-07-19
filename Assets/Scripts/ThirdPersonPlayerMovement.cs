using System;
using UnityEngine;
using MLAPI;

[RequireComponent(typeof(CharacterController), typeof(InputManager))]
public class ThirdPersonPlayerMovement : IPlayerMovement
{
    #region Rotation Attributes
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    #endregion

    private float initialGravity;
    private bool isLocked = false;

    public static event Action<bool> LockPlayer;

    protected override void Awake()
    {
        base.Awake();
        InputManager.AltFire += HandleAltFire;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputManager.AltFire -= HandleAltFire;
    }

    protected override void Start()
    {
        base.Start();
        initialGravity = gravity;
    }

    protected override void Jump()
    {
        if (isLocked) return;
        base.Jump();
    }

    protected override void Move()
    {
        if (isLocked) return;
        if (inputManager.Movement.magnitude >= 0.1f)
        {
            float targetMovementAngle = Mathf.Atan2(inputManager.Movement.x, inputManager.Movement.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float smoothedMovementAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetMovementAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedMovementAngle, 0f);

            Vector3 rotatedMovement = Quaternion.Euler(0f, targetMovementAngle, 0f) * Vector3.forward;
            controller.Move(rotatedMovement.normalized * Time.deltaTime * playerSpeed);
        }
    }

    void HandleAltFire()
    {
        isLocked = !isLocked;
        LockPlayer?.Invoke(isLocked);
    }
}
