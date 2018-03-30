using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        RegisterPlayer();
    }

    private void RegisterPlayer()
    {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents()
    {
        // boucle pour désactiver les components des autres joueurs sur notre instance
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
