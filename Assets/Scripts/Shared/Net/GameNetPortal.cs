using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;

public class GameNetPortal : Singleton<GameNetPortal>
{
    private NetworkManager networkManager;
    private UNetTransport transport;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        transport = networkManager.GetComponent<UNetTransport>();
    }

    public void StartHost(string ip, int port, PlayerData data)
    {
        transport.ConnectAddress = ip;
        transport.ConnectPort = port;
        transport.ServerListenPort = port;

        networkManager.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(data));

        networkManager.StartHost();
        Debug.Log("Started host");
    }

    public void StartClient(string ip, int port, PlayerData data)
    {
        transport.ConnectAddress = ip;
        transport.ConnectPort = port;
        transport.ServerListenPort = port;

        networkManager.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(data));

        networkManager.StartClient();
        Debug.Log("Started client");
    }
}