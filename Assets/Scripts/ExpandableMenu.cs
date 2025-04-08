using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpandableMenu : MonoBehaviour
{
    public Button toggleButton;     //設定按鈕
    public GameObject otherButtonsContainer;    //按下設定按鈕後會彈出來的按鈕們
    public CanvasGroup canvasGroup;             //同上的Canvas Group
    public Image backgroundImage;   //按下去之後會出現的白色背景

    public float animationDuration = 0.3f;  //淡入淡出時間

    private bool isExpanded = false;    //有沒有按下設定按鈕

    void Start()
    {
        Color bgColor = backgroundImage.color;  //關閉白色背景
        bgColor.a = 0f;
        backgroundImage.color = bgColor;

        toggleButton.onClick.AddListener(ToggleMenu);   //賦予設定按鈕一個監聽事件

        
        otherButtonsContainer.SetActive(false); //其他按鈕初始關閉
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ToggleMenu()    //如果設定按鈕被點擊，則出現其他按鈕，再點一下則消失
    {
        isExpanded = !isExpanded;
        StopAllCoroutines();
        StartCoroutine(FadeMenu(isExpanded));
    }

    IEnumerator FadeMenu(bool show)     //淡入淡出的動畫
    {
        if (show) //淡入
        {
            otherButtonsContainer.SetActive(true);
        }

        float startAlpha = canvasGroup.alpha;   //按鈕部分
        float endAlpha = show ? 1 : 0;
        float time = 0;

        Color bgColor = backgroundImage.color;  //背景部分
        float startAlphaBg = bgColor.a;
        float endAlphaBg = show ? 0.5f : 0f; // 同步透明度

        while (time < animationDuration)    //平滑的淡入
        {
            float t = time / animationDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / animationDuration);

            bgColor.a = Mathf.Lerp(startAlphaBg, endAlphaBg, t);
            backgroundImage.color = bgColor;

            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        bgColor.a = endAlphaBg;             //鎖定背景圖
        backgroundImage.color = bgColor;

        canvasGroup.interactable = show;    //鎖定按鈕
        canvasGroup.blocksRaycasts = show;

        if (!show)  //淡出
        {
            otherButtonsContainer.SetActive(false);
        }    

        
    }
}
