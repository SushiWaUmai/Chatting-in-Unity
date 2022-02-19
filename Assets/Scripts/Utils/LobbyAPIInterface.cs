using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

/// <summary>
/// Wrapper for all the interactions with the Lobby API.
/// </summary>
public static class LobbyAPIInterface
{
    private static async void Authenticate()
    {
        // Authenticate
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private const int k_maxLobbiesToShow = 16; // If more are necessary, consider retrieving paginated results or using filters.

    public static async void CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate, Dictionary<string, PlayerDataObject> localUserData, Action<Lobby> onComplete)
    {
        Authenticate();

        CreateLobbyOptions createOptions = new CreateLobbyOptions
        {
            IsPrivate = isPrivate,
            Player = new Player(AuthenticationService.Instance.PlayerId, data: localUserData)
        };
        Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
        onComplete?.Invoke(lobby);
    }

    public static async void DeleteLobbyAsync(string lobbyId, Action onComplete)
    {
        await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
        onComplete?.Invoke();
    }

    public static async void JoinLobbyAsync_ByCode(string lobbyCode, Dictionary<string, PlayerDataObject> localUserData, Action<Lobby> onComplete)
    {
        Authenticate();
        JoinLobbyByCodeOptions joinOptions = new JoinLobbyByCodeOptions { Player = new Player(id: AuthenticationService.Instance.PlayerId, data: localUserData) };
        Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);
        onComplete?.Invoke(lobby);
    }

    public static async void JoinLobbyAsync_ById(string requesterUASId, string lobbyId, Dictionary<string, PlayerDataObject> localUserData, Action<Lobby> onComplete)
    {
        Authenticate();
        JoinLobbyByIdOptions joinOptions = new JoinLobbyByIdOptions { Player = new Player(id: AuthenticationService.Instance.PlayerId, data: localUserData) };
        Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId, joinOptions);
        onComplete?.Invoke(lobby);
    }

    public static async void QuickJoinLobbyAsync(List<QueryFilter> filters, Dictionary<string, PlayerDataObject> localUserData, Action<Lobby> onComplete)
    {
        Authenticate();
        var joinRequest = new QuickJoinLobbyOptions
        {
            Filter = filters,
            Player = new Player(id: AuthenticationService.Instance.PlayerId, data: localUserData)
        };

        Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(joinRequest);
        onComplete?.Invoke(lobby);
    }

    public static async void LeaveLobbyAsync(string lobbyId, Action onComplete)
    {
        Authenticate();
        await Lobbies.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
        onComplete?.Invoke();
    }

    public static async void QueryAllLobbiesAsync(List<QueryFilter> filters, Action<QueryResponse> onComplete)
    {
        QueryLobbiesOptions queryOptions = new QueryLobbiesOptions
        {
            Count = k_maxLobbiesToShow,
            Filters = filters
        };
        QueryResponse res = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);
        onComplete?.Invoke(res);
    }

    public static async void GetLobbyAsync(string lobbyId, Action<Lobby> onComplete)
    {
        Lobby lobby = await Lobbies.Instance.GetLobbyAsync(lobbyId);
        onComplete?.Invoke(lobby);
    }

    public static async void UpdateLobbyAsync(string lobbyId, Dictionary<string, DataObject> data, bool shouldLock, Action<Lobby> onComplete)
    {
        UpdateLobbyOptions updateOptions = new UpdateLobbyOptions { Data = data, IsLocked = shouldLock };
        Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(lobbyId, updateOptions);
        onComplete?.Invoke(lobby);
    }

    public static async void UpdatePlayerAsync(string lobbyId, string playerId, Dictionary<string, PlayerDataObject> data, Action<Lobby> onComplete, string allocationId)
    {
        UpdatePlayerOptions updateOptions = new UpdatePlayerOptions
        {
            Data = data,
            AllocationId = allocationId
        };
        Lobby lobby = await Lobbies.Instance.UpdatePlayerAsync(lobbyId, playerId, updateOptions);
        onComplete?.Invoke(lobby);
    }

    public static async void HeartbeatPlayerAsync(string lobbyId)
    {
        await Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
    }
}
