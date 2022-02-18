using UnityEngine;
using Unity.Netcode;

public class GameNetPortal : Singleton<GameNetPortal>
{
    private NetworkManager networkManager;
    private UnityTransport transport;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        transport = networkManager.GetComponent<UnityTransport>();
    }

    public void StartHost(PlayerData data)
    {
        Debug.Log("Started host");
        ServerGameNetPortal.Instance.StartHost(data);
    }

    public void StartClient(PlayerData data, string joincode)
    {
        Debug.Log("Started client");
        ClientGameNetPortal.Instance.StartClient(data, joincode);
    }
}
