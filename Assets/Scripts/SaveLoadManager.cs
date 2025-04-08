using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject saveLoadPanel;    //保存頁面
    public TextMeshProUGUI panelTitle;  //Save or Load 大文字
    public List<Button> saveLoadButtons = new List<Button>();    //存檔按鈕(們)
    public Button returnButton;         //返回按鈕
    public Button plusButton;           //新增存檔欄位按鈕
    public GameObject saveButtonPrefab; //儲存按鈕的預製體
    public Transform saveLoadButtonParent;    //存放儲存按鈕的容器
    //public GameObject gamePanel;    //用來截圖的
    private int saveIndex = Constants.INITIAL_SAVE_INDEX;

    private bool isSave;    //Save or Load
    //private readonly int totalSlots = Constant

    public static SaveLoadManager Instance { get; private set;} //單例模式

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowSaveLoadUI(bool save)   //顯示Save(1) or Load(0)
    {
        isSave = save;
        panelTitle.text = isSave ? Constants.SAVE_TITLE : Constants.LOAD_TITLE ;
        saveLoadPanel.SetActive(true);
        if(isSave)
            plusButton.gameObject.SetActive(true);
        else    
            plusButton.gameObject.SetActive(false);
        
    }

    public void OnClickAdd()    //將預製體加入到按鈕群組並添加名稱與日期時間
    {
        if (saveButtonPrefab == null || saveLoadButtonParent == null)  //防呆
        {
            Debug.LogWarning("請確認 SaveButton 預製體、SaveLoadButton 和 GamePanel 都有設定！");
            return;
        }

        GameObject newButtonObj = Instantiate(saveButtonPrefab, saveLoadButtonParent);  //新增預製體
        Button newButton = newButtonObj.GetComponent<Button>(); //轉成按鈕
        if (newButton != null)
        {
            // 加入 list
            saveLoadButtons.Add(newButton);

            // 設定 contents 文字
            var contentsText = newButtonObj.transform.Find("Contents")?.GetComponent<TMP_Text>();
            if (contentsText != null)
            {
                contentsText.text = $"{Constants.SAVE_PREFIX}{saveIndex}";
            }

            // 設定時間
            var timeStampText = newButtonObj.transform.Find("TimeStamp")?.GetComponent<TMP_Text>();
            if (timeStampText != null)
            {
                timeStampText.text = DateTime.Now.ToString(Constants.TIMESTAMP_FORMAT);
            }

            // 設定截圖
            // var screenshotImage = newButtonObj.transform.Find("ScreenShot")?.GetComponent<Image>();
            // if (screenshotImage != null)
            // {
            //     StartCoroutine(CaptureScreenshotToImage(screenshotImage));
            // }

            saveIndex++;
        }
    }

    // 擷取 GamePanel 畫面並設到 Image 裡
    // private IEnumerator<UnityEngine.WaitForEndOfFrame> CaptureScreenshotToImage(Image targetImage)
    // {
    //     yield return new WaitForEndOfFrame();

    //     RectTransform rt = gamePanel.GetComponent<RectTransform>();
    //     Vector2 size = rt.rect.size;

    //     // 擷取螢幕畫面（只要 GamePanel 區域）
    //     Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, gamePanel.transform.position);

    //     Texture2D tex = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
    //     tex.ReadPixels(new Rect(screenPos.x, screenPos.y, size.x, size.y), 0, 0);
    //     tex.Apply();

    //     // 轉換為 Sprite 並設定
    //     Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    //     targetImage.sprite = sprite;
    // }

    public void OnClickReturn()     //返回按鈕事件
    {
        saveLoadPanel.SetActive(false);
    }
    
}
