using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField connectionInputField;
    [SerializeField] private GameObject[] uiMenu;

    private enum MenuState
    {
        MainMenu,
        LobbyMenu
    }

    private void Start()
    {
        SwitchMenu(MenuState.MainMenu);
        ClientGameNetPortal.Instance.OnConnectGame += OnJoinLobby;
    }

    private void OnDestroy()
    {
        if(ClientGameNetPortal.Instance)
            ClientGameNetPortal.Instance.OnConnectGame -= OnJoinLobby;
    }


    public void HostGame()
    {
        (string ip, int port) = GetIPAndPort(connectionInputField.text);

        GameNetPortal.Instance.StartHost(ip, port);
    }

    public void JoinGame()
    {
        (string ip, int port) = GetIPAndPort(connectionInputField.text);

        GameNetPortal.Instance.StartClient(ip, port);
    }

    private void OnJoinLobby()
    {
        SwitchMenu(MenuState.LobbyMenu);
    }

    public void SwitchMenu(int menuState)
    {
        foreach (GameObject menu in uiMenu)
        {
            menu.SetActive(false);
        }

        uiMenu[menuState].SetActive(true);

        Debug.Log($"Switching to Menu: {nameof(MenuState)}.{System.Enum.GetName(typeof(MenuState), menuState)}");
    }

    private void SwitchMenu(MenuState menuState)
    {
        SwitchMenu((int)menuState);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private (string, int) GetIPAndPort(string input)
    {
        string[] ipPort = connectionInputField.text.Split(':');
        string ip = ipPort[0];

        int port;
        if (ipPort.Length > 1 && int.TryParse(ipPort[1], out port))
        {
            return (ip, port);
        }

        Debug.LogError("Invalid IP or port");
        return ("", 0);
    }
}
