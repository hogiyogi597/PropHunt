using UnityEngine;
using MLAPI;
using Cinemachine;

public class PlayerNetworkedComponentManager : NetworkBehaviour
{
    void Start()
    {
        TogglePlayerCamera(IsLocalPlayer);
    }

    private void TogglePlayerCamera(bool newValue)
    {
        GetComponentInChildren<Camera>().enabled = newValue;
        GetComponentInChildren<CinemachineVirtualCameraBase>().enabled = newValue;
        GetComponentInChildren<AudioListener>().enabled = newValue;
        GetComponentInChildren<CinemachineBrain>().enabled = newValue;
    }
}
