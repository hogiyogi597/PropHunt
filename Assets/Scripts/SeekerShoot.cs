using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class SeekerShoot : NetworkBehaviour
{
    [SerializeField] private float shootDistance = 50f;
    [SerializeField] private int damageValue = 20;

    private Camera playerCamera;

    public static event Action<GameObject, int> ShotObject;

    void Awake()
    {
        InputManager.PrimaryFire += HandleFireInput;
    }

    void OnDestroy()
    {
        InputManager.PrimaryFire -= HandleFireInput;
    }

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void HandleFireInput()
    {
        if (IsLocalPlayer)
        {
            ShootServerRpc();
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootDistance))
        {
            ShotObject(hit.transform.gameObject, damageValue);
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * shootDistance, Color.red);
    }
}
