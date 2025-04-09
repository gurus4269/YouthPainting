using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using System.IO;
using System.Xml;


public class SaveLoadManager : MonoBehaviour
{
    public GameObject saveLoadPanel;    //保存頁面
    public TextMeshProUGUI panelTitle;  //Save or Load 大文字
    public List<Button> saveLoadButtons = new List<Button>();    //存檔按鈕(們)
    public Button returnButton;         //返回按鈕
    public Button plusButton;           //新增存檔欄位按鈕
    public GameObject saveButtonPrefab; //儲存按鈕的預製體
    public Transform saveLoadButtonParent;    //存放儲存按鈕的容器
    public ScreenShotter screenShotter;     //照相機(用來截圖)
    private int saveIndex = Constants.INITIAL_SAVE_INDEX;   //存檔序號
    private string saveFolderPath;      //存檔路徑
    private byte[] screenshotData;      //儲存截圖資料

    private bool isSave;    //Save or Load
    //private readonly int totalSlots = Constant

    public static SaveLoadManager Instance { get; private set;} //單例模式

    private void Awake()
    {
        if(Instance == null)    //刪除多的單例，如果沒有單例，則自己成為單例
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitializedSaveFilePath();  //初始化儲存路徑
    }
    public void InitializedSaveFilePath()   //初始化儲存路徑
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH);    //預設路徑 + 設定的資料夾名稱
        if(!Directory.Exists(saveFolderPath))   //防呆
        {
            Directory.CreateDirectory(saveFolderPath);
            
        }
    }
    public void ShowSaveLoadUI(bool save)   //顯示Save(1) or Load(0)
    {
        isSave = save;  
        panelTitle.text = isSave ? Constants.SAVE_TITLE : Constants.LOAD_TITLE ;
        saveLoadPanel.SetActive(true);
        if(isSave)  //Save有添加按鈕
        {
            plusButton.gameObject.SetActive(true);
        }  
        else    //Load沒有添加按鈕
        {
            plusButton.gameObject.SetActive(false);
        }  
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
            // 加入到list
            saveLoadButtons.Add(newButton);

            // 設定 contents 文字
            var contentsText = newButtonObj.transform.Find("Contents")?.GetComponent<TMP_Text>();
            if (contentsText != null)
            {
                contentsText.text = $"{Dialog.Instance.currentSpeakingContent}";  //名稱 + 序號
            }

            // 設定時間
            var timeStampText = newButtonObj.transform.Find("TimeStamp")?.GetComponent<TMP_Text>();
            if (timeStampText != null)
            {
                timeStampText.text = DateTime.Now.ToString(Constants.TIMESTAMP_FORMAT);     //年月日時分秒
            }

            //加入截圖
            var screenShotImage = newButtonObj.transform.Find("ScreenShot")?.GetComponent<Image>();
            if (screenShotImage != null)
            {
                // 把 screenshotData (byte[]) 轉成 Texture2D 再轉成 Sprite
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(screenshotData); // 載入 byte[]
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                screenShotImage.sprite = sprite;
            }

            SaveData saveData = new SaveData    //存放的東西
            {
                currentSpeakingContentID = Dialog.Instance.currentSpeakingContentID,    //當前對話的序號
                currentSpeakingContent = Dialog.Instance.currentSpeakingContent,    //當前的對話   
                currentBackground = Dialog.Instance.currentBackground,      //最後的背景
                currentFilter = Dialog.Instance.currentFilter,  //最後的濾鏡
                currentPositions = Dialog.Instance.positions,   //所有位置上的人
                timeStamp = timeStampText.text,     //年月日時分秒
                screenshotData = screenshotData //遊戲畫面截圖
            };

            string fileName = $"{Constants.SAVE_PREFIX}{saveIndex}.json";   //檔名 + 序號
            string savePath = Path.Combine(saveFolderPath, fileName);       //路徑 + 黨名 + 序號

            string json = JsonUtility.ToJson(saveData, true); // true 表示格式化（縮排）
            File.WriteAllText(savePath, json);

            saveIndex++;
        }
    }

    [Serializable]
    public class SaveData
    {        
        public int currentSpeakingContentID;    //最後一句對話ID
        public string currentSpeakingContent;   //最後一句對話
        public string currentBackground;    //最後的背景
        public bool currentFilter;  //最後的回憶濾鏡
        public string[] currentPositions = new string[5];   //相對應的角色位置
        public string timeStamp;    //年月日時分秒
        public byte[] screenshotData;   //畫面截圖
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
    public void OnClickSave()       //顯示Save面板
    {
        Dialog.Instance.OnclickCloseUI();   //關閉右側設定欄以及下方對話框
        Texture2D screenshot = screenShotter.CaptureScreenShot();   //截圖
        screenshotData = screenshot.EncodeToPNG();  //轉成圖片
        ShowSaveLoadUI(true);   //顯示存檔UI
        LoadAllSaveSlots();
        Dialog.Instance.OpenUI();   //開啟右側設定欄以及下方對話框

    }
    public void OnClickLoad()       //顯示Load面板
    {
        ShowSaveLoadUI(false);
        LoadAllSaveSlots();
    }
    public void OnClickReturn()     //返回按鈕事件
    {
        saveLoadPanel.SetActive(false);
    }
    public void LoadAllSaveSlots()  //加載存檔
    {
        // 清除舊按鈕
        foreach (Transform child in saveLoadButtonParent)
        {
            Destroy(child.gameObject);
        }
        saveLoadButtons.Clear();

        // 確保資料夾存在(防呆)
        if (!Directory.Exists(saveFolderPath))
        {
            Debug.LogWarning("找不到存檔資料夾：" + saveFolderPath);
            return;
        }

        // 遍歷所有 json 存檔
        string[] files = Directory.GetFiles(saveFolderPath, "*.json");
        
        saveIndex = files.Length + 1;

        foreach (string file in files)  
        {
            string json = File.ReadAllText(file);   //讀檔
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);   //依照屬性分開

            GameObject newButtonObj = Instantiate(saveButtonPrefab, saveLoadButtonParent);  //利用預製體生成物件
            Button newButton = newButtonObj.GetComponent<Button>(); //轉成按鈕
            if (newButton != null)
            {
                saveLoadButtons.Add(newButton); //加入到list

                var contentsText = newButtonObj.transform.Find("Contents")?.GetComponent<TMP_Text>();   //加載對話
                if (contentsText != null)
                {
                    contentsText.text = TruncateMixedText(saveData.currentSpeakingContent, 14, 12);
                }

                var timeStampText = newButtonObj.transform.Find("TimeStamp")?.GetComponent<TMP_Text>(); //加載時間戳記
                if (timeStampText != null)
                {
                    DateTime lastWrite = File.GetLastWriteTime(file);
                    timeStampText.text = lastWrite.ToString(Constants.TIMESTAMP_FORMAT);
                }

                var screenShotImage = newButtonObj.transform.Find("ScreenShot")?.GetComponent<Image>(); //加載截圖
                if (screenShotImage != null && saveData.screenshotData != null)
                {
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(saveData.screenshotData);
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    screenShotImage.sprite = sprite;
                }

                string pathCopy = file; // 避免閉包錯誤
                newButton.onClick.AddListener(() =>
                {
                    LoadGameFromFile(pathCopy);
                });

            }
        }
    }
    
    private bool IsFullWidth(char c)    // 判斷一個字是否為全形（CJK Unicode 範圍內）
    {
        return c >= 0x3000 && c <= 0x9FFF;
    }

    private string TruncateMixedText(string input, int maxWidth, int preserveWidth)
    {
        int width = 0;
        string result = "";

        foreach (char c in input)
        {
            int charWidth = IsFullWidth(c) ? 2 : 1; //計算長度，全形兩單位，半形一單位

            if (width + charWidth > preserveWidth)  //如果超過preserveWidth，停止計算
                break;

            result += c;    //保留目前的字
            width += charWidth; //長度 ++ (或 += 2)
        }

        if (GetMixedWidth(input) > maxWidth)    //如果原字串寬度 > maxWidth，就加上 "..."
            result += "...";

        return result;
    }
    
    private int GetMixedWidth(string input) // 計算字串總寬度（全形 = 2，半形 = 1）
    {
        int total = 0;
        foreach (char c in input)
        {
            total += IsFullWidth(c) ? 2 : 1;
        }
        return total;
    }
    private void LoadGameFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("找不到存檔檔案：" + filePath);
            return;
        }
        Dialog.Instance.gamePanel.SetActive(true);

        string json = File.ReadAllText(filePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // ➤ 還原對話 ID
        Dialog.Instance.LoadGame(saveData.currentSpeakingContentID);

        // ➤ 還原背景與濾鏡
        Dialog.Instance.UpdateBackGroundImage(saveData.currentBackground, saveData.currentFilter);

        // ➤ 還原角色圖（依位置）
        for (int i = 0; i < saveData.currentPositions.Length; i++)
        {
            string name = saveData.currentPositions[i];
            if (!string.IsNullOrEmpty(name))
            {
                Dialog.Instance.UpdateImage(name, i);
            }
        }

        // 顯示遊戲畫面
        MenuManager.Instance.menuPanel.SetActive(false);
        saveLoadPanel.SetActive(false);

        Debug.Log("成功載入存檔：" + filePath);
    }

}
