using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public List<string> historyList = new List<string>();   //前X行的歷史紀錄

    private string autosavePath;//存檔路徑（Checkpoint 與 Autosave 的資料夾）
    private string autosaveDir; //路徑 + 檔名

    public static GameMaster Instance { get; private set; } //單例模式，跨場景保留
    private void Awake()
    {
        if (Instance == null)
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
        
    }

    //以下為切換Scene
    public void LoadGameScene()     //-> Game      NewGame、Continue按鈕用
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