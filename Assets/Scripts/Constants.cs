using UnityEngine;

public class Constants
{
    //劇本路徑
    public const string STORY_PATH = "Text/Test";   //劇本

    //讀取文件用的符號
    public const string DIALOG = "#";       //一般對話
    public const string CHOICE = "&";       //選項
    public const string BACKGROUND = "@";   //更改背景符號
    public const string END = "END";        //結束符號

    //常數
    public const int DIALOGINDEX = 0;   //開始讀取的行數

    //存檔用
    public const string SAVE_PREFIX = "Save_File_";     //存檔的開頭
    public const int INITIAL_SAVE_INDEX = 1;    //存檔的序號
    public const string TIMESTAMP_FORMAT = "yyyy/MM/dd HH:mm:ss";   //存檔的時間格式
    public const string SAVE_FILE_PATH = "Saves";
    public const string SAVE_EXTENSION = ".json"; 

    //UI文字
    public const string SAVE_TITLE = "SAVE";    //儲存字樣
    public const string LOAD_TITLE = "LOAD";    //載入字樣

    //場景
    public const string SCENE_MENU = "MenuScene";
    public const string SCENE_GAME = "GameScene";
    public const string SCENE_SAVELOAD = "SaveLoadScene";
}
