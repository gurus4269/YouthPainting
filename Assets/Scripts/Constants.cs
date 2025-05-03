using UnityEngine;

public class Constants
{
    //劇本路徑
    public const string STORY_PATH = "Text/YouthPaintingChapter1";   //劇本

    //讀取文件用的符號
    public const string DIALOG = "#";       //一般對話
    public const string CHOICE = "&";       //選項
    public const string WAIT = "$";         //等待
    public const string NARRATION = "*";    //旁白
    public const string BACKGROUND = "@";   //更改背景符號
    public const string END = "END";        //結束符號

    //常數
    public const int DIALOGINDEX = 0;   //開始讀取的行數
    public const float DIALOGSPEEDSLOW = 2f; //慢速換行速度
    public const float DIALOGSPEEDFAST = 0.2f; //慢速換行速度

    //存檔用
    public const string SAVE_PREFIX = "Save_File_";     //存檔的開頭
    public const int INITIAL_SAVE_INDEX = 1;    //存檔的序號
    public const string TIMESTAMP_FORMAT = "yyyy/MM/dd HH:mm:ss";   //存檔的時間格式
    public const string SAVE_FILE_PATH = "Saves";
    public const string SAVE_EXTENSION = ".json"; 
    public const int HISTORYLINES = 50;

    //UI文字
    public const string SAVE_TITLE = "SAVE";    //儲存字樣
    public const string LOAD_TITLE = "LOAD";    //載入字樣

    //場景
    public const string SCENE_MENU = "MenuScene";   //首頁
    public const string SCENE_GAME = "GameScene";   //主要遊戲畫面
    public const string SCENE_SAVELOAD = "SaveLoadScene";   //存檔頁面
    public const string SCENE_SETTING = "SettingScene";     //設定頁面
    public const string SCENE_GALLERY = "GalleryScene";     //畫廊頁面
    public const string SCENE_CHARACTER = "CharacterScene";     //角色介紹頁面
    public const string SCENE_END = "EndScene";     //結束頁面
}
