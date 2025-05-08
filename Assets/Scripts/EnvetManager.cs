using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class EventManager : MonoBehaviour
{//*****寫備註*****///
    private string src_event => Path.Combine(Application.streamingAssetsPath, $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{GameMaster.Instance.currentChapterId.ToString()}{Constants.SAVE_FILE_EVENT_PART2}");   //放checkpoint的資料夾路徑(在存檔處)
    private string src_trigger => Path.Combine(Application.streamingAssetsPath, $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{GameMaster.Instance.currentChapterId.ToString()}{Constants.SAVE_FILE_TRIGGER_PART2}");   //放checkpoint的資料夾路徑(在存檔處)
    private string CheckpointFolder => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH_CHECKPOINT);   //放checkpoint的資料夾路徑(在存檔處)
    
    private string EventJsonPath => Path.Combine(   //第X章的event檔的路徑
        CheckpointFolder,
        $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{GameMaster.Instance.currentChapterId.ToString()}{Constants.SAVE_FILE_EVENT_PART2}"
    );

    private string TriggerJsonPath => Path.Combine( //第X章的trigger檔的路徑
        CheckpointFolder,
        $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{GameMaster.Instance.currentChapterId.ToString()}{Constants.SAVE_FILE_TRIGGER_PART2}"
    );

    

    private Dictionary<int, EventData> events = new Dictionary<int, EventData>();   //event字典
    private List<TriggerData> triggers = new List<TriggerData>();                   //trigger list(trigger的index不唯一，所以不能用字典)
    public static EventManager Instance { get; private set;} //單例模式
    void Awake()
    {
        //確認只有一個實例，多的刪掉
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (!File.Exists(EventJsonPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(EventJsonPath));
            File.Copy(src_event, EventJsonPath);
        }
        if (!File.Exists(TriggerJsonPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TriggerJsonPath));
            File.Copy(src_trigger, TriggerJsonPath);
        }

        if (!Directory.Exists(CheckpointFolder))    //檢查檔案路徑，若沒有，則創建
        {
            Directory.CreateDirectory(CheckpointFolder);
        }
        
        LoadEvents();       // 載入事件資料
        LoadTriggers();     // 載入觸發條件
        TryTriggerEvents(); // 啟動所有 Lock 為 0 的事件
    }

    
    void LoadEvents()   // 載入事件資料
    {
        if (!File.Exists(EventJsonPath))    //檢查檔案是否存在
        {
            Debug.LogWarning("事件 JSON 檔案不存在：" + EventJsonPath);
            return;
        }

        string json = File.ReadAllText(EventJsonPath);
        EventList list = JsonUtility.FromJson<EventList>(json);
        foreach (var e in list.events)
        {
            events[e.Index] = e;
        }
    }

    void LoadTriggers() // 載入觸發條件
    {
        string path = TriggerJsonPath;

        if (!File.Exists(path))
        {
            Debug.LogWarning("觸發 JSON 檔案不存在：" + path);
            return;
        }

        string json = File.ReadAllText(path);
        TriggerList list = JsonUtility.FromJson<TriggerList>(json);
        triggers = list.triggers;
    }

    // 執行事件
    public void ActivateEvent(int index)
    {
        if (!events.ContainsKey(index)) return;

        var evt = events[index];
        if (evt.Triggered) return;

        Debug.Log($"[觸發事件] {evt.Index}: {evt.TextName}");
        evt.Triggered = true;

        // 處理連鎖觸發
        foreach (var t in triggers.Where(t => t.Index == index && t.OpenOrClose == 0))
        {
            if (events.TryGetValue(t.Target, out var target))
            {
                if (t.Key)
                    target.KeyCount++;

                if (target.KeyCount >= target.Lock)
                    ActivateEvent(target.Index);
            }
        }
    }

    // 觸發所有初始事件（Lock == 0）
    void TryTriggerEvents()
    {
        foreach (var e in events.Values)
        {
            if (e.Lock == 0 && !e.Triggered)
                ActivateEvent(e.Index);
        }
    }
}
