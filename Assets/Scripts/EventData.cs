using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventData
{
    public int Index;          // 事件 ID
    public string TextName;   // 劇本名稱
    public string Type;       // 事件型態（Auto / Scenario / Button）
    public string SubType;    // 子型態（場景名、人名等）
    public int OpenLock;      // 需要多少Key來打開
    public int KeyCount;  // 當前收到的 Key 數量
    public bool CloseLock;    // 關閉的鎖
}

[System.Serializable]   //他的list型態
public class EventList
{
    public List<EventData> events;
}
