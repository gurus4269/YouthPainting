using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackGroundSlot : MonoBehaviour
{
    [Header("背景圖片元件")]
    public Image backgroundImage;

    [Header("記憶濾鏡物件")]
    public GameObject memoryFilter;

    [Header("淡入動畫時間（秒）")]
    public float fadeInTime = 0.3f;

    private Coroutine currentFade;

    
    public void SetBackground(Sprite sprite, bool isMemory = false) // 設定背景圖片，並決定是否開啟記憶濾鏡
    {
        if (backgroundImage == null || sprite == null)  //檢查是否有圖片
        {
            Debug.LogWarning($"{name}：背景圖片未設定或傳入為 null");
            return;
        }

        backgroundImage.sprite = sprite;    //取得圖片
        SetAlpha(0f);   // 先設為透明

        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        currentFade = StartCoroutine(FadeIn());

        // 控制回憶濾鏡顯示
        if (memoryFilter != null)
        {
            memoryFilter.SetActive(isMemory);
        }
    }

    
    public void Clear() //清除背景
    {
        backgroundImage.sprite = null;
        SetAlpha(0f);

        if (memoryFilter != null)
            memoryFilter.SetActive(false);
    }

    
    private IEnumerator FadeIn()    // 淡入動畫
    {
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeInTime);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(1f);
    }

    
    private void SetAlpha(float a)  // 設定透明度
    {
        Color c = backgroundImage.color;
        c.a = a;
        backgroundImage.color = c;
    }
}
