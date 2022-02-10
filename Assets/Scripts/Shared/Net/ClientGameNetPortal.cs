using UnityEngine;
using Unity.Netcode;

public class ClientGameNetPortal : Singleton<ClientGameNetPortal>
{
    private NetworkManager networkManager;
    public event System.Action OnConnectGame;
    public event System.Action OnDisconnectGame;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        networkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
        networkManager.OnServerStarted += OnNetworkReady;
    }

    private void OnDestroy()
    {
        if (networkManager)
        {
            networkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            networkManager.OnServerStarted -= OnNetworkReady;
        }
    }

    private void OnNetworkReady()
    {
        if (!networkManager.IsClient)
        {
            enabled = false;
        }
    }

    private void OnClientConnectedCallback(ulong connectionID)
    {
        Debug.Log("Client connected");

        if (networkManager.LocalClientId == connectionID)
            OnConnectGame?.Invoke();
    }

    private void OnClientDisconnectCallback(ulong connectionID)
    {
        Debug.Log("Client disconnected");

        if (networkManager.LocalClientId == connectionID)
            OnDisconnectGame?.Invoke();
    }
}