using UnityEngine;
using MLAPI;

public abstract class IPlayerMovement : NetworkBehaviour
{
    [Header("Base Attributes")]
    [SerializeField] protected float playerSpeed = 12.0f;

    #region Jump Attributes    
    [Header("Jump Attributes")]
    [SerializeField] protected float jumpHeight = 3f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected float gravity = -50f;
    [SerializeField] protected float maxFallSpeed = -20f;

    protected float groundDistance = 0.4f;
    protected bool isGrounded = true;
    protected float velocityY = 0f;
    #endregion

    #region ObjectReferences
    protected CharacterController controller;
    protected Camera playerCamera;
    protected InputManager inputManager;
    #endregion

    protected virtual void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        InputManager.Jump += HandleJumpInput;

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }


    protected virtual void OnDestroy()
    {
        InputManager.Jump -= HandleJumpInput;
    }

    protected virtual void Start()
    {
        if (IsLocalPlayer)
        {
            inputManager = GetComponent<InputManager>();
            controller = GetComponent<CharacterController>();
        }
    }

    protected virtual void Update()
    {
        if (IsLocalPlayer)
        {
            CheckIsGrounded();
            Move();
            Jump();
        }
    }

    protected void CheckIsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    protected abstract void Move();

    protected virtual void Jump()
    {
        if (isGrounded && velocityY < maxFallSpeed)
        {
            velocityY = maxFallSpeed;
        }

        velocityY += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, velocityY, 0f) * Time.deltaTime);
    }

    protected void HandleJumpInput()
    {
        if (IsLocalPlayer && isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}