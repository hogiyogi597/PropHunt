using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class NetworkStarter : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.StartHost();
    }
}
