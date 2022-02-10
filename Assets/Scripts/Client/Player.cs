using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    
    private void Start()
    {
        if (IsLocalPlayer)
            LocalPlayer = this;
    }

    [ServerRpc]
    public void SendMessageServerRpc(string message)
    {
        ReceiveMessageClientRpc(message);
    }

    [ClientRpc]
    public void ReceiveMessageClientRpc(string message)
    {
        MessageDisplay.Instance.DisplayMessage(message);
    }
}
