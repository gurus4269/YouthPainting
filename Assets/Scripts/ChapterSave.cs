using System;
using System.Collections.Generic;

[Serializable] // Unity 內建 JSON 儲存用
public class ChapterSave
{
    public int priority;    //優先級
    public string name;    //人物編號
    public string place;   //哪個地圖
    public string text;    //對話文本
    public bool show;   //是否顯示
}
