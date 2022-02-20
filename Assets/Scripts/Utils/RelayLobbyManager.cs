using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies.Models;
using System.Collections;
using UnityEngine;
using Unity.Services.Lobbies;

public class RelayLobbyManager : Singleton<RelayLobbyManager>
{
    public UnityTransport Transport { get; private set; }

    private void Start()
    {
        Transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    }

    /// <summary>
    /// HostGame allocate a Relay server and returns needed data to host the game
    /// </summary>
    /// <param name="maxConn">The maximum number the Relay can have</param>
    /// <returns>A Task returning the needed hosting data</returns>
    public async Task<RelayHostData> HostGame(int maxConn, PlayerData playerData)
    {
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        //Ask Unity Services to allocate a Relay server
        Allocation allocation = await Unity.Services.Relay.Relay.Instance.CreateAllocationAsync(maxConn);

        //Populate the hosting data
        RelayHostData data = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        //Retrieve the Relay join code for our clients to join our party
        data.JoinCode = await Unity.Services.Relay.Relay.Instance.GetJoinCodeAsync(data.AllocationID);

        Transport.SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes,
                data.Key, data.ConnectionData);

        // create lobby
        Lobby lobby = await LobbyAPIInterface.CreateLobbyAsync("test", maxConn, true, data.JoinCode, allocation.AllocationId.ToString(), null);

        // Heartbeat the lobby every 15 seconds.
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

        return data;
    }

    /// <summary>
    /// Join a Relay server based on the JoinCode received from the Host or Server
    /// </summary>
    /// <param name="joinCode">The join code generated on the host or server</param>
    /// <returns>All the necessary data to connect</returns>
    public async Task<RelayJoinData> JoinGame(string joinCode, PlayerData playerData)
    {
        //Initialize the Unity Services engine
        await UnityServices.InitializeAsync();
        //Always autheticate your users beforehand
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            //If not already logged, log the user in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        //Ask Unity Services for allocation data based on a join code
        JoinAllocation allocation = await Unity.Services.Relay.Relay.Instance.JoinAllocationAsync(joinCode);

        //Populate the joining data
        RelayJoinData data = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        Transport.SetRelayServerData(data.IPv4Address, data.Port, data.AllocationIDBytes,
            data.Key, data.ConnectionData, data.HostConnectionData);

        LobbyAPIInterface.JoinLobbyAsyncByCode(joinCode, null, null);

        return data;
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}


public struct RelayHostData
{
    public string JoinCode;
    public string IPv4Address;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] Key;
}

public struct RelayJoinData
{
    public string JoinCode;
    public string IPv4Address;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] HostConnectionData;
    public byte[] Key;
}
