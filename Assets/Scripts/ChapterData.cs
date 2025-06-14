using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChapterData
{
    public int Index;   // 章節 ID
    public bool Lock;   // 章節鎖

    
}

[System.Serializable]   //他的list型態
public class ChapterList
{
    public List<ChapterData> chapters;
}
