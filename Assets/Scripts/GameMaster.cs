using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public string lastSceneName;
    //from Dialog
    public TextAsset dialogDataFile;     //對話文件(.csv)
    //以下用作存檔
    public string currentSpeakingContent;  //目前對話內容(針對有對話的句子)
    public int currentSpeakingContentID = Constants.DIALOGINDEX;  //目前對話內容序號
    public string currentBackground;    //目前背景
    public bool currentFilter = false;  //目前有沒有添加回憶濾鏡
    public string[] positions = new string[5];  //儲存5個位置的人
    public string timeStamp;    //時間戳記
    public List<string> historyList = new List<string>();   //前X行的歷史紀錄
    public byte[] screenshotData;   //圖片(截圖)資訊
    //到這邊

    //from Menu
    public bool hasStarted = false;    //判斷是否已經開始(1)，未開始(0) from MenuManager

    //from SaveLoad
    public bool isSave;     //1 -> save, 0 -> load 
    public int saveIndex = Constants.INITIAL_SAVE_INDEX;   //存檔序號

    public static GameMaster Instance { get; private set;} //單例模式

    private void Awake()
    {
        if(Instance == null)    //刪除多的單例，如果沒有單例，則自己成為單例
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);     //跨場景保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogDataFile = Resources.Load<TextAsset>(Constants.STORY_PATH);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadGameScene(int _id)     //-> Game      NewGame、Continue按鈕用
    {
        currentSpeakingContentID = _id;
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }
    public void LoadMenuScene()     //-> Menu      Home按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_MENU);
    } 
    public void LoadSettingScene()     //-> Menu      Home按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_SETTING);
    }
    public void LoadGalleryScene()     //-> Gallery     Home按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_GALLERY);
    }
    public void LoadCharacterScene()     //-> Character    Home按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_CHARACTER);
    }
    public void LoadSaveLoadScene()  //-> SaveLoad  Load按鈕、LoadGame按鈕用
    {
        lastSceneName = SceneManager.GetActiveScene().name; // 記錄當前場景
        SceneManager.LoadScene(Constants.SCENE_SAVELOAD);
    }
    public void LoadEndScene()     //-> End     
    {
        SceneManager.LoadScene(Constants.SCENE_END);
    }

    //方便當前資料   1.目前對話(有用的對話)  2.該對話的ID    3.目前的背景    4.是否有記憶濾鏡    5.每個位置上的角色 6.時間戳記  7.圖片(截圖)
    public void SetPatameter(SaveData _newSave)
    {
        currentSpeakingContent = _newSave.saveCurrentSpeakingContent;
        currentSpeakingContentID = _newSave.saveCurrentSpeakingContentID;
        currentBackground = _newSave.saveCurrentBackground;
        currentFilter = _newSave.saveCurrentFilter;
        positions = _newSave.saveCurrentPositions;
        timeStamp = _newSave.saveTimeStamp;    //年月日時分秒
        historyList = _newSave.saveHistoryList;     //歷史紀錄
        screenshotData = _newSave.saveScreenshotData;   //畫面截圖

        LoadGameScene(currentSpeakingContentID);
    }

    public void OnClickSave()       //顯示Save面板
    {
        Dialog.Instance.OnclickCloseUI();   //關閉右側設定欄以及下方對話框
        Texture2D screenshot = Dialog.Instance.screenShotter.CaptureScreenShot();   //截圖
        screenshotData = screenshot.EncodeToPNG();  //轉成圖片
        Dialog.Instance.OpenUI();   //開啟右側設定欄以及下方對話框
        isSave = true;
        LoadSaveLoadScene(); //顯示存檔UI
        

    }
    public void OnClickLoad()       //顯示Load面板
    {
        isSave = false;
        LoadSaveLoadScene(); //顯示存檔UI
        SaveLoadManager.Instance.LoadAllSaveSlots();
    }

}
