using UnityEngine;
using TMPro;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messsageText;
    [SerializeField] private TextMeshProUGUI usernameText;
    public void Init(string text, string user)
    {
        messsageText.text = text;
        usernameText.text = user;
    }
}