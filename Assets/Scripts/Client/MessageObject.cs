using UnityEngine;
using TMPro;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messsageText;
    public void Init(string text)
    {
        messsageText.text = text;
    }
}