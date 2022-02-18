using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField usernameInputField;

    [SerializeField] private GameObject[] uiMenu;
    [SerializeField] private TextMeshProUGUI joinCodeDisplay;

    private enum MenuState
    {
        MainMenu,
        LobbyMenu
    }

    private void Start()
    {
        SwitchMenu(MenuState.MainMenu);
        ClientGameNetPortal.Instance.OnConnectGame += OnJoinLobby;
        ServerGameNetPortal.Instance.OnHostGame += OnHostGame;
    }

    private void OnDestroy()
    {
        if (ClientGameNetPortal.Exists)
            ClientGameNetPortal.Instance.OnConnectGame -= OnJoinLobby;
        if (ServerGameNetPortal.Exists)
            ServerGameNetPortal.Instance.OnHostGame -= OnHostGame;
    }


    public void HostGame()
    {
        PlayerData playerData = new PlayerData(usernameInputField.text);
        GameNetPortal.Instance.StartHost(playerData);
    }

    public void OnHostGame(RelayHostData data)
    {
        joinCodeDisplay.text = data.JoinCode;
    }

    public void JoinGame()
    {
        string joincode = joinCodeInputField.text;
        PlayerData playerData = new PlayerData(usernameInputField.text);

        GameNetPortal.Instance.StartClient(playerData, joincode);
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
}
