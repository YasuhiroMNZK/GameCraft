using UnityEngine;
using UnityEngine.UI;

public class ChattingManager : MonoBehaviour
{
    [Header("mytext")]
    public Text[] myTexts;

    [Header("npctext")]
    public Text[] npcTexts;
    public ScrollRect scrollRect;

    [Header("scrollPositions")]
    public float[] scrollPositions;
    private int chatIndex = 0;

    public void AddChat(string myMessage, string npcMessage)
    {
        if (chatIndex >= myTexts.Length)
        {
            return;
        }

        myTexts[chatIndex].text = myMessage;
        npcTexts[chatIndex].text = npcMessage;

        chatIndex++;
    }
    public void ScrollToStep(int step)
    {
        Canvas.ForceUpdateCanvases();

        if (step < scrollPositions.Length)
        {
            scrollRect.verticalNormalizedPosition = scrollPositions[step];
        }
    }
}