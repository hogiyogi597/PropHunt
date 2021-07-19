using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private InputMaster inputMaster;

    public Vector2 Look { get; private set; }
    public Vector3 Movement { get; private set; }
    public float RotationDirection { get; private set; }

    public static event Action PrimaryFire;
    public static event Action AltFire;
    public static event Action Jump;

    void Awake()
    {
        inputMaster = new InputMaster();

        inputMaster.Player.Move.performed += HandleMove;
        inputMaster.Player.Move.canceled += HandleMove;

        inputMaster.Player.Fire.started += HandlePrimaryFire;

        inputMaster.Player.AltFire.started += HandleAltFire;

        inputMaster.Player.Jump.started += HandleJump;

        inputMaster.Player.Rotate.started += HandleRotate;
        inputMaster.Player.Rotate.canceled += HandleRotate;

        inputMaster.Player.Look.performed += HandleLook;
        inputMaster.Player.Look.canceled += HandleLook;
    }

    void OnDestroy()
    {
        inputMaster.Player.Move.performed -= HandleMove;
        inputMaster.Player.Move.canceled -= HandleMove;

        inputMaster.Player.Fire.started -= HandlePrimaryFire;

        inputMaster.Player.AltFire.started -= HandleAltFire;

        inputMaster.Player.Jump.started -= HandleJump;

        inputMaster.Player.Rotate.started -= HandleRotate;
        inputMaster.Player.Rotate.canceled -= HandleRotate;

        inputMaster.Player.Look.performed -= HandleLook;
        inputMaster.Player.Look.canceled -= HandleLook;
    }

    void OnEnable()
    {
        inputMaster.Enable();
    }

    void OnDisable()
    {
        inputMaster.Disable();
    }

    void HandleMove(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        Movement = new Vector3(rawInput.x, 0, rawInput.y);
    }

    void HandleLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    void HandleRotate(InputAction.CallbackContext context)
    {
        float rawInput = context.ReadValue<float>();
        RotationDirection = Mathf.Ceil(rawInput);
    }

    Action<InputAction.CallbackContext> HandlePrimaryFire = _ => PrimaryFire?.Invoke();
    Action<InputAction.CallbackContext> HandleAltFire = _ => AltFire?.Invoke();
    Action<InputAction.CallbackContext> HandleJump = _ => Jump?.Invoke();
}
