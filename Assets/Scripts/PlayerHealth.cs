using System.Collections;
using System;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.Spawning;

[RequireComponent(typeof(Collider), typeof(NetworkObject))]
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private static int initialHealth = 100;
    private NetworkVariable<int> health = new NetworkVariable<int>(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly }, initialHealth);

    public static event Action PlayerDied;

    void Awake()
    {
        SeekerShoot.ShotObject += HandleShotObject;
        PlayerDied += HandlePlayerDied;
    }

    void OnDestroy()
    {
        SeekerShoot.ShotObject -= HandleShotObject;
        PlayerDied -= HandlePlayerDied;
    }

    void HandleShotObject(GameObject shotObject, int appliedDamage)
    {
        if (shotObject == this.gameObject)
        {
            ApplyDamageServerRpc(appliedDamage);
        }
    }

    // FIXME: For testing only
    void HandlePlayerDied()
    {
        ulong networkId = GetComponent<NetworkObject>().NetworkObjectId;
        DestroyDeadPlayerServerRpc(networkId);
    }

    [ServerRpc]
    void DestroyDeadPlayerServerRpc(ulong networkId)
    {
        NetworkObject objectToDestroy = NetworkSpawnManager.SpawnedObjects[networkId];
        Destroy(objectToDestroy.gameObject);
    }

    [ServerRpc]
    void ApplyDamageServerRpc(int damage)
    {
        health.Value -= damage;
        Debug.Log($"Took {damage} damage. Health is now {health.Value}");
        if (health.Value <= 0)
        {
            health.Value = 0;
            PlayerDied?.Invoke();
        }
    }
}
