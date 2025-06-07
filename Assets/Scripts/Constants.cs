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
    public const string LEAVE = "LEAVE";    //暫時結束符號

    //常數
    public const int DIALOGINDEX = 0;   //開始讀取的行數
    public const float DIALOGSPEEDSLOW = 2f; //慢速換行速度
    public const float DIALOGSPEEDFAST = 0.2f; //慢速換行速度

    //存檔用
    public const string SAVE_FILE_PATH_AUTO = "AutoSave";   //自動存檔的資料夾名稱
    public const string SAVE_FILE_PATH_CHECKPOINT = "Checkpoint";   //固定存檔的資料夾名稱
    public const string SAVE_EXTENSION = ".json";   //存檔副檔名
    public const int HISTORYLINES = 50; //歷史紀錄最多幾行
    public const string SAVE_FILE_EVENT_AND_TRIGGER_PART1 = "Checkpoint_";  //固定存檔的檔案名稱part1
    public const string SAVE_FILE_EVENT_PART2 = "_events.json";             //固定存檔的檔案名稱part2 event部分(完整檔名Checkpoint_數字_events.json)
    public const string SAVE_FILE_TRIGGER_PART2 = "_triggers.json";         //固定存檔的檔案名稱part2 trigger部分(完整檔名Checkpoint_數字_triggers.json)

    //UI文字
    public const string SAVE_TITLE = "SAVE";    //儲存字樣
    public const string LOAD_TITLE = "LOAD";    //載入字樣

    //場景
    public const string SCENE_MENU = "MenuScene";   //首頁
    public const string SCENE_GAME = "GameScene";   //主要遊戲畫面
    public const string SCENE_CHAPTER = "ChapterScene";   //存檔頁面
    public const string SCENE_SETTING = "SettingScene";     //設定頁面
    public const string SCENE_GALLERY = "GalleryScene";     //畫廊頁面
    public const string SCENE_CHARACTER = "CharacterScene";     //角色介紹頁面
    public const string SCENE_END = "EndScene";     //結束頁面
}
