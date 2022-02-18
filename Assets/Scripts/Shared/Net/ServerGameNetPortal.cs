using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerGameNetPortal : Singleton<ServerGameNetPortal>
{
    private RelayHostData data;
    private Dictionary<ulong, PlayerData> playerData = new Dictionary<ulong, PlayerData>();
    private NetworkManager networkManager;
    public event System.Action<RelayHostData> OnHostGame;

    private void Start()
    {
        networkManager = NetworkManager.Singleton;
        networkManager.OnServerStarted += OnNetworkReady;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void OnDestroy()
    {
        if (networkManager)
        {
            networkManager.ConnectionApprovalCallback -= ApprovalCheck;
            networkManager.OnServerStarted -= OnNetworkReady;
        }
    }

    public async void StartHost(PlayerData playerData)
    {
        networkManager.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(playerData));

        data = await RelayUtil.Instance.HostGame(10);
        OnHostGame?.Invoke(data);
        networkManager.StartHost();
    }


    private void OnNetworkReady()
    {
        if (!networkManager.IsServer)
        {
            enabled = false;
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        Debug.Log(clientId);
        PlayerData data = JsonUtility.FromJson<PlayerData>(System.Text.Encoding.ASCII.GetString(connectionData));
        AddPlayer(clientId, data);

        callback(true, null, true, null, null);
    }

    public void AddPlayer(ulong clientId, PlayerData data)
    {
        playerData[clientId] = data;
    }

    public void RemovePlayer(ulong clientId)
    {
        playerData.Remove(clientId);
    }

    public PlayerData GetPlayerData(ulong clientId)
    {
        return playerData[clientId];
    }
}

