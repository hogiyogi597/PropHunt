using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class ModelRotator : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    public Transform objectToRotate;

    private Quaternion currentRotation;
    private bool isLocked = false;

    private InputManager inputManager;

    void Awake()
    {
        ThirdPersonPlayerMovement.LockPlayer += HandlePlayerLock;
    }

    void OnDestroy()
    {
        ThirdPersonPlayerMovement.LockPlayer -= HandlePlayerLock;
    }

    void Start()
    {
        inputManager = GetComponentInParent<InputManager>();
        currentRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = currentRotation;
        if (isLocked) return;
        if (IsLocalPlayer && inputManager.RotationDirection != 0f)
        {
            objectToRotate.Rotate(Vector3.up * rotationSpeed * inputManager.RotationDirection);
            currentRotation = transform.rotation;
        }
    }

    public void HandlePlayerLock(bool lockedState)
    {
        isLocked = lockedState;
    }
}
