using System;
using System.Collections.Generic;

[Serializable] // Unity 內建 JSON 儲存用
public class SaveData
{
    public int saveCurrentSpeakingContentID;    //最後一句對話ID
    public string saveCurrentSpeakingContent;   //最後一句對話
    public string saveCurrentBackground;    //最後的背景
    public bool saveCurrentFilter;  //最後的回憶濾鏡
    public string[] saveCurrentPositions = new string[5];   //相對應的角色位置
    public string saveTimeStamp;    //年月日時分秒
    public List<string> saveHistoryList = new List<string>();   //前X行的歷史紀錄
    public byte[] saveScreenshotData;   //畫面截圖
}