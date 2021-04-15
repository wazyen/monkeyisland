using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerButton : MonoBehaviour
{
    private static GameObject ButtonsContainer;

    void Start()
    {
        GetNecessaryStuff();
    }

    private static void GetNecessaryStuff()
    {
        if (ButtonsContainer == null)
            ButtonsContainer = GameObject.Find("answers-buttons");
    }

    public static void ScrollUp()
    {
        Vector3 pos = ButtonsContainer.GetComponent<RectTransform>().anchoredPosition;
        pos.y = Mathf.Max(0, pos.y - 18);
        ButtonsContainer.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public static void ScrollDown()
    {
        int childCount = ButtonsContainer.transform.childCount;
        if (childCount <= 6)
            return;
        Vector2 pos = ButtonsContainer.GetComponent<RectTransform>().anchoredPosition;
        pos.y = Mathf.Min(pos.y + 18, 18 * (childCount - 6));
        ButtonsContainer.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public static void scrollToTop()
    {
        GetNecessaryStuff();
        Vector3 pos = ButtonsContainer.GetComponent<RectTransform>().anchoredPosition;
        pos.y = 0;
        ButtonsContainer.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
