using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSlot : MonoBehaviour
{
    [Header("角色圖片元件")]
    public Image characterImage;

    [Header("淡入時間（秒）")]
    public float fadeInDuration = 0.3f;

    private Coroutine fadeCoroutine;

    
    public void SetCharacter(Sprite sprite) // 顯示角色圖片，並淡入顯示
    {
        if (characterImage == null || sprite == null)
        {
            Debug.LogWarning($"{name}：SetCharacter 中有 null，Image 或 Sprite 沒設定");
            return;
        }

        characterImage.sprite = sprite;

        
        SetAlpha(0f);   // 先讓圖片完全透明

        
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);    // 啟動淡入動畫
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    
    public void Clear() // 清除角色圖片（將圖片變成透明）
    {
        if (characterImage == null) return;

        characterImage.sprite = null;
        SetAlpha(0f); // 保持圖片物件啟用，只是透明
    }

    
    private IEnumerator FadeIn()    // 淡入動畫：從透明到完全顯示
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f); // 確保最後完全顯示
    }

    
    private void SetAlpha(float alpha)  // 設定圖片透明度
    {
        if (characterImage == null) return;

        Color color = characterImage.color;
        color.a = alpha;
        characterImage.color = color;
    }
}
