using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public int currentChapterId;    //當前章節ID
    public List<ChapterSave> currentCharacters = new List<ChapterSave>();   //儲存每個角色的狀態資料（位置、優先度、對話等）
    public List<bool> paramFlags = new List<bool>();    //條件布林參數（如任務完成、選項觸發等）
    public List<string> historyList = new List<string>();   //前X行的歷史紀錄

    private string checkpointPath;  //存檔路徑（Checkpoint 與 Autosave 的資料夾）
    private string autosavePath;
    private string autosaveDir; //路徑 + 檔名
    private string checkpointDir;

    public static GameMaster Instance { get; private set; } //單例模式，跨場景保留
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitializeSavePaths();  //初始化
    }

    private void InitializeSavePaths()//初始化
    {
        //初始化存檔路徑
        autosavePath = Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH_AUTO);    //預設路徑 + 設定的資料夾名稱
        if(!Directory.Exists(autosavePath))   //防呆
        {
            Directory.CreateDirectory(autosavePath);
        }
        checkpointPath = Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH_CHECKPOINT);    //預設路徑 + 設定的資料夾名稱
        if(!Directory.Exists(checkpointPath))   //防呆
        {
            Directory.CreateDirectory(checkpointPath);
        }
    }
    public void LoadCheckpoint()    //載入指定章節的 Checkpoint（只讀取，不覆蓋）
    {
        checkpointDir = Path.Combine(checkpointPath, $"checkpoint_{currentChapterId}.json");    //先設定路徑

        if (File.Exists(checkpointPath))    //再讀取
        {
            string json = File.ReadAllText(checkpointPath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            ApplySaveData(data);
            Debug.Log("  已載入章節 Checkpoint！");
        }
        else    //防呆
        {
            Debug.LogWarning("  找不到 Checkpoint 檔案！");
        }
    }

    public void SaveAuto()  //儲存Autosave（每次關閉/切換場景時儲存）
    {
        autosaveDir = Path.Combine(autosavePath, "AutoSave.json");
        SaveData data = CreateSaveData();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(autosaveDir, json);
        Debug.Log("Autosave 儲存完成！");
    }

    public void LoadAutoSave()  //載入 Autosave 存檔
    {
        if (File.Exists(autosaveDir))
        {
            string json = File.ReadAllText(autosaveDir);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            ApplySaveData(data);
            Debug.Log("Autosave 載入完成！");
        }
        else
        {
            Debug.LogWarning("找不到 Autosave 存檔！");
        }
    }
    
    private SaveData CreateSaveData()   //將當前狀態轉成 SaveData_new 結構（準備寫入檔案）
    {
        return new SaveData
        {
            chapterId = currentChapterId,
            characters = new List<ChapterSave>(currentCharacters),
            parameters = new List<bool>(paramFlags)
        };
    }

    private void ApplySaveData(SaveData data)   //將 SaveData_new 結構套用回 GameMaster 狀態（從 JSON 還原）
    {
        currentChapterId = data.chapterId;
        currentCharacters = new List<ChapterSave>(data.characters);
        paramFlags = new List<bool>(data.parameters);
    }


    //以下為切換Scene
    public void LoadGameScene(int _id)     //-> Game      NewGame、Continue按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }
    public void LoadMenuScene()     //-> Menu      Home按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_MENU);
    } 
    public void LoadSettingScene()     //-> Setting      Home按鈕用
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
    public void LoadChapterScene()  //-> Chapter  Load按鈕、LoadGame按鈕用
    {
        SceneManager.LoadScene(Constants.SCENE_CHAPTER);
    }
    public void LoadEndScene()     //-> End     
    {
        SceneManager.LoadScene(Constants.SCENE_END);
    }

}