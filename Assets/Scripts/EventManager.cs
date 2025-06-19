using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using JetBrains.Annotations;

public class EventManager : MonoBehaviour
{//*****寫備註*****///
    public class ButtonListenerInfo //添加在按鈕上的對話事件用的資料結構
    {
        public Button button;
        public UnityEngine.Events.UnityAction action;
    }

    public string CheckpointFolder;
    public string EventJsonPath;
    public string TriggerJsonPath;
    public string srcEvent;
    public string srcTrigger;

    private Dictionary<string, int> scenarioNum = new Dictionary<string, int>();    //地圖字典
    public List<GameObject> mapPanels = new List<GameObject>();                     //地圖list，兩個合在一起才有用
    private Dictionary<string, int> characterNum = new Dictionary<string, int>();   //人物字典
    public List<GameObject> characterGameObject = new List<GameObject>();           //人物list，兩個合在一起才有用
    private Dictionary<int, EventData> events = new Dictionary<int, EventData>();   //event字典
    private Dictionary<int, ButtonListenerInfo> buttonListeners = new Dictionary<int, ButtonListenerInfo>();    //按鈕上的event字典

    private List<TriggerData> triggers = new List<TriggerData>();                   //trigger list(trigger的index不唯一，所以不能用字典)
    public static EventManager Instance { get; private set; } //單例模式
    void Awake()
    {
        //確認只有一個實例，多的刪掉
        if (Instance == null)
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
        //初始化地圖上的按鈕
        scenarioNum["3-A"] = 0;
        scenarioNum["3-B"] = 1;
        scenarioNum["3-C"] = 2;
        scenarioNum["3-D"] = 3;
        scenarioNum["hallway"] = 4;
        scenarioNum["teachers office"] = 5;
        scenarioNum["club room"] = 6;
        scenarioNum["Mioru room"] = 7;
        scenarioNum["Fuku room"] = 8;
        scenarioNum["Kasuno room"] = 9;
        scenarioNum["Yuzuki room"] = 10;
        scenarioNum["Aoi room"] = 11;
        scenarioNum["dormitory hallway"] = 12;
        scenarioNum["hall"] = 13;

        //初始化人物的按鈕
        characterNum["澪瑠"] = 0;
        characterNum["芙玖"] = 1;
        characterNum["霞乃"] = 2;
        characterNum["柚希"] = 3;
        characterNum["曉"] = 4;
        characterNum["茜"] = 5;
        characterNum["校長"] = 6;
        characterNum["小泉老師"] = 7;


        srcEvent = GetSrcEventPath();
        srcTrigger = GetSrcTriggerPath();
        CheckpointFolder = GetCheckpointFolder();
        EventJsonPath = GetEventJsonPath();
        TriggerJsonPath = GetTriggerJsonPath();
        Debug.Log(EventJsonPath);

        // 建立存檔資料夾
        if (!Directory.Exists(CheckpointFolder))
        {
            Directory.CreateDirectory(CheckpointFolder);
        }

        // 複製 event 檔案
        if (!File.Exists(EventJsonPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(EventJsonPath));
            File.Copy(srcEvent, EventJsonPath);
            Debug.Log("已複製事件檔案到存檔資料夾。");
        }

        // 複製 trigger 檔案
        if (!File.Exists(TriggerJsonPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TriggerJsonPath));
            File.Copy(srcTrigger, TriggerJsonPath);
            Debug.Log("已複製觸發器檔案到存檔資料夾。");
        }

        LoadEvents();       // 載入事件資料
        LoadTriggers();     // 載入觸發條件
        TryTriggerEvents(); // 啟動所有 OpenLock 為 0 的事件
    }


    void LoadEvents()   // 載入事件資料
    {
        if (!File.Exists(EventJsonPath))    //檢查檔案是否存在
        {
            Debug.LogWarning("事件 JSON 檔案不存在：" + EventJsonPath);
            return;
        }

        string json = File.ReadAllText(EventJsonPath);              //讀檔
        EventList list = JsonUtility.FromJson<EventList>(json);     //
        foreach (var e in list.events)                              //放入字典
        {
            events[e.Index] = e;
        }
    }

    void LoadTriggers() // 載入觸發條件
    {
        if (!File.Exists(TriggerJsonPath))  //檢查檔案是否存在
        {
            Debug.LogWarning("觸發 JSON 檔案不存在：" + TriggerJsonPath);
            return;
        }

        string json = File.ReadAllText(TriggerJsonPath);                //讀檔
        TriggerList list = JsonUtility.FromJson<TriggerList>(json);
        triggers = list.triggers;                                       //放入trigger list
    }

    public void ActivateEvent(int index)    // 執行事件
    {
        if (!events.ContainsKey(index)) //檢查事件是否存在於字典中
        {
            return;
        }

        var evt = events[index];        //檢查是否被已經被觸發

        // SaveEventsAndTriggers();//先做存檔
        Debug.Log($"[觸發事件] {evt.Index}: {evt.TextName}");

        //evt.NowTriggered = true;   //標記為已被觸發

        TextAsset textAsset = Resources.Load<TextAsset>($"Text/{evt.TextName}");//讀劇本

        if (evt.Type == "Auto")     //用作要被立刻觸發的對話(開頭、結束、滿足某條件後)，只會執行一次，不用刪事件
        {
            Dialog.Instance.TriggerDialog(textAsset, 0);
            //evt.CloseLock = true;   //標記為已被觸發
        }
        else if (evt.Type == "Scenario")    //用作進入場景後的對話，執行完後需立即刪除
        {
            GameObject scenarioObj = mapPanels[scenarioNum[evt.SubType]]; // SubType 對應按鈕物件名稱
            if (scenarioObj != null)
            {
                var btn = scenarioObj.GetComponent<UnityEngine.UI.Button>();    //找到該物件上的按鈕屬性
                if (btn == null)                                                //如果本身沒有添加按鈕屬性
                {
                    btn = scenarioObj.AddComponent<UnityEngine.UI.Button>();    //就賦予他一個
                }

                if (!buttonListeners.ContainsKey(evt.Index))    //給他賦予事件後，放入字典中，之後方便查找
                {
                    UnityEngine.Events.UnityAction action = () => Dialog.Instance.TriggerDialog(textAsset, 0);
                    btn.onClick.AddListener(action);
                    buttonListeners[evt.Index] = new ButtonListenerInfo
                    {
                        button = btn,
                        action = action
                    };
                    //btn.onClick.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"找不到對應的 Button 元件：{evt.SubType}");
            }
        }
        else if (evt.Type == "Button")          //同場景
        {
            GameObject characterObj = characterGameObject[characterNum[evt.SubType]]; // SubType 對應按鈕物件名稱
            if (characterObj != null)
            {
                var btn = characterObj.GetComponent<UnityEngine.UI.Button>();
                if (btn == null)
                {
                    btn = characterObj.AddComponent<UnityEngine.UI.Button>();
                }

                if (!buttonListeners.ContainsKey(evt.Index))
                {
                    UnityEngine.Events.UnityAction action = () => Dialog.Instance.TriggerDialog(textAsset, 0);
                    btn.onClick.AddListener(action);
                    buttonListeners[evt.Index] = new ButtonListenerInfo
                    {
                        button = btn,
                        action = action
                    };
                    //btn.onClick.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"找不到對應的 Button 元件：{evt.SubType}");
            }
        }
        SaveEventsAndTriggers();    //存檔
    }

    void TryTriggerEvents()     // 觸發所有初始事件（OpenLock == 0）
    {
        foreach (var e in events.Values)
        {
            if (e.OpenLock == e.KeyCount && !e.CloseLock)
            {
                ActivateEvent(e.Index);
            }
        }
    }


    public void CheckOpen(int _index)   //開鎖OR上鎖
    {
        foreach (var t in triggers.Where(t => t.Index == _index))  //尋找所有triggers，來源為此事件的
        {
            if (t.OpenOrClose == 0)  //開鎖部分
            {
                Debug.Log("開鎖");
                if (events.TryGetValue(t.Target, out var target))
                {
                    if (t.Key)  //開鎖
                    {
                        target.KeyCount++;
                        t.Key = false;  //這邊要寫回
                    }

                    if (target.KeyCount >= target.OpenLock) //檢查被開鎖的
                    {
                        ActivateEvent(target.Index);
                    }
                }
            }
            else if (t.OpenOrClose == 1) //上鎖部分
            {

                if (events.TryGetValue(t.Target, out var target))
                {
                    if (t.Key)  //開鎖
                    {
                        Debug.Log("上鎖");
                        RemoveButtonListener(target.Index);  //移除事件
                        t.Key = false;  //這邊要寫回
                    }
                }
            }
            SaveEventsAndTriggers();    //存檔
        }
    }

    public void RemoveButtonListener(int eventIndex)    //移除按鈕監聽事件
    {
        events[eventIndex].CloseLock = true;
        if (buttonListeners.TryGetValue(eventIndex, out var info) && info.button != null)
        {
            SaveEventsAndTriggers();    //存檔
            info.button.onClick.RemoveListener(info.action);
            buttonListeners.Remove(eventIndex);
            Debug.Log($"移除了按鈕事件：事件 index = {eventIndex}");
        }
    }

    public void SaveEventsAndTriggers() //存檔
    {
        // 儲存 Events
        EventList eventList = new EventList
        {
            events = events.Values.ToList()
        };
        string eventJson = JsonUtility.ToJson(eventList, true); // 排版美化
        File.WriteAllText(EventJsonPath, eventJson);
        Debug.Log($"事件已儲存到：{EventJsonPath}");

        // 儲存 Triggers
        TriggerList triggerList = new TriggerList
        {
            triggers = triggers
        };
        string triggerJson = JsonUtility.ToJson(triggerList, true);
        File.WriteAllText(TriggerJsonPath, triggerJson);
        Debug.Log($"觸發器已儲存到：{TriggerJsonPath}");
    }

    //以下是路徑
    private string GetSrcEventPath()    //存在Assset中的checkpoint_event檔
    {
        return Path.Combine(
            Application.streamingAssetsPath,
            $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{Constants.CURRENT_CHPATER_ID}{Constants.SAVE_FILE_EVENT_PART2}"
        );
    }

    private string GetSrcTriggerPath()  //存在Assset中的checkpoint_trigger檔
    {
        return Path.Combine(
            Application.streamingAssetsPath,
            $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{Constants.CURRENT_CHPATER_ID}{Constants.SAVE_FILE_TRIGGER_PART2}"
        );
    }

    private string GetCheckpointFolder()    //放checkpoint的資料夾路徑(在存檔處)
    {
        return Application.persistentDataPath;
    }

    private string GetEventJsonPath()       //第X章的event檔的路徑(在存檔處)
    {
        return Path.Combine(
            GetCheckpointFolder(),
            $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{Constants.CURRENT_CHPATER_ID}{Constants.SAVE_FILE_EVENT_PART2}"
        );
    }

    private string GetTriggerJsonPath()     //第X章的trigger檔的路徑(在存檔處)
    {
        return Path.Combine(
            GetCheckpointFolder(),
            $"{Constants.SAVE_FILE_EVENT_AND_TRIGGER_PART1}{Constants.CURRENT_CHPATER_ID}{Constants.SAVE_FILE_TRIGGER_PART2}"
        );
    }

    public void DeleteCheckPoint()
    {
        string eventFile = GetEventJsonPath();      //event的存檔
        string triggerFile = GetTriggerJsonPath();  //trigger的存檔

        if (File.Exists(eventFile))         // event 檢查，有的畫刪除，沒有的話報錯
        {
            File.Delete(eventFile);
            Debug.Log("存檔已刪除：" + eventFile);
        }
        else
        {
            Debug.LogWarning("找不到要刪除的存檔：" + eventFile);
        }

        if (File.Exists(triggerFile))       // trigger 檢查，有的畫刪除，沒有的話報錯
        {
            File.Delete(triggerFile);
            Debug.Log("存檔已刪除：" + triggerFile);
        }
        else
        {
            Debug.LogWarning("找不到要刪除的存檔：" + triggerFile);
        }

    }
}

