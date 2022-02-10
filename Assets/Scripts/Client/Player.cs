using Unity.Netcode;
using UnityEngine;

public struct PlayerData
{
    public string username;

    public PlayerData(string username)
    {
        this.username = username;
    }
}

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;

    private void Start()
    {
        if (IsLocalPlayer)
            LocalPlayer = this;
    }

    [ServerRpc]
    public void SendMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        ulong playerID = rpcParams.Receive.SenderClientId;
        Debug.Log(playerID);
        PlayerData data = ServerGameNetPortal.Instance.GetPlayerData(playerID);
        ReceiveMessageClientRpc(message, data.username);
    }

    [ClientRpc]
    public void ReceiveMessageClientRpc(string message, string username)
    {
        MessageDisplay.Instance.DisplayMessage(message, username);
    }
}
