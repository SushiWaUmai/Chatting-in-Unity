using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;

public class GameNetPortal : NetworkSingleton<GameNetPortal>
{
    private NetworkManager networkManager;
    private UNetTransport transport;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        transport = networkManager.GetComponent<UNetTransport>();
    }

    public void StartHost(string ip, int port)
    {
        transport.ConnectAddress = ip;
        transport.ConnectPort = port;
        transport.ServerListenPort = port;

        networkManager.StartHost();
        Debug.Log("Started host");
    }

    public void StartClient(string ip, int port)
    {
        transport.ConnectAddress = ip;
        transport.ConnectPort = port;
        transport.ServerListenPort = port;

        networkManager.StartClient();
        Debug.Log("Started client");
    }
}