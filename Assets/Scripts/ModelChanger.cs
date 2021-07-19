using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

[RequireComponent(typeof(InputManager))]
public class ModelChanger : NetworkBehaviour
{
    public MeshFilter playerMesh;
    public List<Mesh> models;
    public Material modelMaterial;

    private InputManager inputManager;

    [SerializeField] private int currentModel = 0;
    private List<Mesh> runtimeModels;

    private bool isLocked = false;

    void Awake()
    {
        InputManager.PrimaryFire += HandleChangeModel;
        ThirdPersonPlayerMovement.LockPlayer += HandlePlayerLock;
    }

    void OnDestroy()
    {
        InputManager.PrimaryFire -= HandleChangeModel;
        ThirdPersonPlayerMovement.LockPlayer -= HandlePlayerLock;
    }

    void Start()
    {
        Renderer meshRenderer = playerMesh.GetComponent<Renderer>();
        meshRenderer.enabled = false;
        inputManager = GetComponent<InputManager>();
        runtimeModels = new List<Mesh>();
        foreach (var model in models)
        {
            runtimeModels.Add(model);
        }

        meshRenderer.material = modelMaterial;
        playerMesh.sharedMesh = runtimeModels[currentModel];
        Debug.Log($"Set current mesh to index {currentModel}");
        meshRenderer.enabled = true;
    }

    void HandleChangeModel()
    {
        if (isLocked) return;
        if (IsLocalPlayer)
        {
            currentModel = (currentModel + 1) % runtimeModels.Count;
            playerMesh.sharedMesh = runtimeModels[currentModel];
        }
    }

    public void HandlePlayerLock(bool lockedState)
    {
        isLocked = lockedState;
    }
}
