using System;
using System.Collections.Generic;

[Serializable] // Unity 內建 JSON 儲存用
public class SaveData
{
    public int chapterId;   //章節編號
    public List<ChapterSave> characters = new List<ChapterSave>();  //角色事件清單
    public List<bool> parameters = new List<bool>();    //條件參數
}
