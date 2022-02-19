using UnityEngine;
using TMPro;

public class MessageInput : MonoBehaviour
{
    private TMP_InputField messageInputField;

    private void Start()
    {
        messageInputField = GetComponent<TMP_InputField>();
        messageInputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnSubmit(string message)
    {
        messageInputField.text = string.Empty;

        // focus on input field
        messageInputField.ActivateInputField();

        PlayerBehaviour.LocalPlayer.SendMessageServerRpc(message);
    }
}