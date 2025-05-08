using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TriggerData
{
    public int Index;         // 來源事件的 ID（誰觸發）
    public int OpenOrClose;   // 0 表示開啟，1 表示關閉
    public int Target;        // 目標事件 ID（誰被觸發）
    public bool Key;          // 是否提供一把 key（代表邏輯依賴）
}

[System.Serializable]       //他的list型態
public class TriggerList
{
    public List<TriggerData> triggers;
}
