using System;

[Serializable] // Unity 內建 JSON 儲存用
public class SaveData
{
    public string currentSpeakingContent; // 存對話內容
    public byte[] screenshotData;         // 存截圖資料（PNG byte 陣列）
}