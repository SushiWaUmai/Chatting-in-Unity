using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : Singleton<MessageDisplay>
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private RectTransform messageContainer;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private int maxMessages = 100;

    private Queue<MessageObject> messageQueue = new Queue<MessageObject>();

    public void DisplayMessage(string text, string user)
    {
        if (messageQueue.Count < maxMessages)
        {
            GameObject go = Instantiate(messagePrefab, messageContainer);
            MessageObject mo = go.GetComponent<MessageObject>();
            messageQueue.Enqueue(mo);
            mo.Init(text, user);

            // increase the size of the container by 115
            messageContainer.sizeDelta = new Vector2(messageContainer.sizeDelta.x, messageContainer.sizeDelta.y + 115);

            // scroll to bottom
            scrollRect.verticalNormalizedPosition = 0;
        }
        else
        {
            MessageObject mo = messageQueue.Dequeue();
            mo.Init(text, user);

            // set sibling index to the last child
            mo.transform.SetSiblingIndex(messageContainer.childCount - 1);
            
            messageQueue.Enqueue(mo);
        }
    }
}
