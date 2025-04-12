using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using System.IO;
//using UnityEngine.UIElements;

public class Dialog : MonoBehaviour
{
    //public static TextAsset dialogDataFile; //對話文件(.csv)
    public CharacterSlot leftSlot, middleLeftSlot, middleSlot, middleRightSlot, rightSlot;  //角色圖像放的位置
    private Dictionary<int, CharacterSlot> positionSlots = new Dictionary<int, CharacterSlot>();    //角色圖像放的位置字典
    public TMP_Text nameText, dialogText;    //對話框中的角色名字欄位、對話內容欄位
    public Button next;     //下一步按鈕
    public GameObject choice;   //選項按鈕預製體
    public Transform choiceGroup;   //放選項的區域
    public BackGroundSlot backgroundSlot;   //背景圖片放置區
    //public GameObject gamePanel;    //遊戲畫面
    public GameObject settingUI, textUI;    //側邊按鈕, 下方對話框
    public ScreenShotter screenShotter;     //照相機(用來截圖)

    // //以下用作存檔
    public int dialogIndex = Constants.DIALOGINDEX;    //當前的對話行數
    // public string currentSpeakingContent;  //目前對話內容(針對有對話的句子)
    // public int currentSpeakingContentID;  //目前對話內容序號
    // public string currentBackground;    //目前背景
    // public bool currentFilter;  //目前有沒有添加回憶濾鏡
    // public string[] positions = new string[5];  //儲存5個位置的人
    

    // //到這邊

    public List<Sprite> sprites = new List<Sprite>();    //角色圖片列表
    private Dictionary<string, Sprite> imageDic= new Dictionary<string, Sprite>();  //角色圖片字典
    public List<Sprite> backGroundSprites = new List<Sprite>();    //背景圖片列表
    private Dictionary<string, Sprite> backGroundDic= new Dictionary<string, Sprite>();  //背景圖片字典
    private string[] dialogRows;    //按照行分開的文件
    public static Dialog Instance { get; private set;} //單例模式

    private void Awake() 
    {
        GameMaster.Instance.dialogDataFile = Resources.Load<TextAsset>(Constants.STORY_PATH);
        if (GameMaster.Instance.dialogDataFile == null)
            Debug.LogError("找不到對話檔案！");
        
        LoadCharacterSprites();    // 載入角色圖
        LoadBackgroundSprites();   // 載入背景圖

        //放圖片的位置
        positionSlots[0] = leftSlot;
        positionSlots[1] = middleLeftSlot;
        positionSlots[2] = middleSlot;
        positionSlots[3] = middleRightSlot;
        positionSlots[4] = rightSlot;
        
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
    // Start is called before the first frame update
    void Start()
    {
        LoadGame(GameMaster.Instance.currentSpeakingContentID);
        if(GameMaster.Instance.currentSpeakingContentID != 0)
        {
            //➤ 還原背景與濾鏡
            UpdateBackGroundImage(GameMaster.Instance.currentBackground, GameMaster.Instance.currentFilter);

            //➤ 還原角色圖（依位置）
            for (int i = 0; i < GameMaster.Instance.positions.Length; i++)
            {
                string name = GameMaster.Instance.positions[i];
                if (!string.IsNullOrEmpty(name))
                {
                    UpdateImage(name, i);
                }
            }
        }
        Camera mainCam = Camera.main;
        screenShotter = mainCam.GetComponent<ScreenShotter>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //點擊事件：如果現在TextUI以及Setting被關掉，則打開他們
        {
            OpenUI();
        }
    }

    public void LoadGame(int loadId) //透過調用來執行存檔遊戲
    {
        leftSlot.Clear();   //所有位置的圖片都先設成透明
        middleLeftSlot.Clear();
        middleSlot.Clear();
        middleRightSlot.Clear();
        rightSlot.Clear();

        ReadText(GameMaster.Instance.dialogDataFile);   //讀劇本
        dialogIndex = loadId;    //初始化行號
        ShowDialogRow();    //讀下一行
    }
    public void UpdateText(string _name, string _text) //更新人名與對話
    {
        nameText.text = _name;  //把人物名字放進 namebar
        dialogText.text = _text;    //把對話放進 dialogbar
    }
    public void UpdateImage(string _name, int _place)   //更新人物立繪
    {
        if (!imageDic.ContainsKey(_name))   //防呆
        {
            Debug.LogWarning($"角色 {_name} 不存在於 imageDic 中！");
            return;
        }

        ClearCharacter(_place, _name);  // 清理不同配置的圖
        CharacterNoSame( _name );       //不同位置的相同圖
        
        GameMaster.Instance.positions[_place] = _name;

        // 顯示角色圖片
        if (positionSlots.ContainsKey(_place))
        {
            positionSlots[_place].SetCharacter(imageDic[_name]);
        }
    }

    public void UpdateBackGroundImage(string _bgKey, bool _isMemory = false)    //更新背景
    {
        if (!backGroundDic.ContainsKey(_bgKey))     //防呆
        {
            Debug.LogWarning($"背景圖片 {_bgKey} 不存在！");
            return;
        }

        GameMaster.Instance.currentBackground = _bgKey;
        GameMaster.Instance.currentFilter = _isMemory;

        backgroundSlot.SetBackground(backGroundDic[_bgKey], _isMemory);
    }

    public void ReadText(TextAsset _textAsset)  //把文件分割成行
    {
        dialogRows = _textAsset.text.Split('\n');  
    }
    public void ShowDialogRow() //主要處理的地方
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');  //按照逗號分開成每個元素
            if(cells[0] == Constants.DIALOG && int .Parse(cells[1]) == dialogIndex)  //#是一般對話
            {
                UpdateImage(cells[2], int.Parse(cells[3]));     
                UpdateText(cells[2], cells[4]);

                GameMaster.Instance.currentSpeakingContent = cells[4];  //儲存最後一句對話(針對有對話的句子)
                GameMaster.Instance.currentSpeakingContentID = int.Parse(cells[1]);  //儲存最後一句對話 

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
                break;
            }
            else if(cells[0] == Constants.CHOICE && int .Parse(cells[1]) == dialogIndex) //選項序號
            {
                next.gameObject.SetActive(false);   //關掉下一步按鈕
                GenerateChoice(i);  //生成選項1
            }
            else if (cells[0] == Constants.BACKGROUND && int.Parse(cells[1]) == dialogIndex) //更改背景
            {
                string bgKey = cells[6];
                bool isMemory =  bool.Parse(cells[7]);
                leftSlot.Clear();
                middleLeftSlot.Clear();
                middleSlot.Clear();
                middleRightSlot.Clear();
                rightSlot.Clear();
                UpdateBackGroundImage(bgKey, isMemory);
                for (int j = 0; j < GameMaster.Instance.positions.Length; j++)
                {
                    GameMaster.Instance.positions[j] = null;
                }

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
            }
            else if(cells[0] == Constants.END && int .Parse(cells[1]) == dialogIndex) //結束
            {
                Debug.Log("劇情結束");
            }
        }
    }
    public void OnClickNext()   //下一步按鈕
    {
        ShowDialogRow();    //讀下一行
    }
    public void OnclickCloseUI()    //關閉UI
    {
        settingUI.SetActive(false); //關掉右邊設定欄位
        textUI.SetActive(false);    //關掉下方對話框
    }
    public void OpenUI()    //開啟UI
    {
        settingUI.SetActive(true);  //關掉右邊設定欄位
        textUI.SetActive(true);     //關掉下方對話框
    }
    public void OnclickHome()   //返回開始畫面
    {
        GameMaster.Instance.LoadMenuScene();
    }

    public void GenerateChoice(int _index)  //生成選項
    {
        string[] cells = dialogRows [_index].Split(',');    //按照逗號分開成每個元素
        if ( cells[0] == "&")   //確認是選項
        {
            GameObject button = Instantiate(choice, choiceGroup);   //將選項預製體加入到選項群組中
            button.GetComponentInChildren<TMP_Text>().text = cells[4];   //給選項添加文字
            button.GetComponent<Button>().onClick.AddListener   //給選項添加觸發事件
                (
                    delegate 
                    { 
                        OnChoiceClick(int.Parse(cells[5]));
                    } 
                );    
            GenerateChoice( _index + 1 );
        }
        
    }
    public void OnChoiceClick(int _id)  //選項觸發事件(跳到回答的那行)
    {
        dialogIndex = _id;
        ShowDialogRow();
        for (int i = 0; i < choiceGroup.childCount ; i++)   //消除選項
        {
            Destroy(choiceGroup.GetChild(i).gameObject);
        }
        next.gameObject.SetActive(true);
    }
    // public void OptionEffect(string _effect, int _param, string _target) //之後再設計效果
    // {
    //     if(_effect == )
    // }
    public void ClearCharacter(int _pos, string _name)    //避免位置錯誤
    {
        if( _pos % 2 == 0)  //左中右三邊配置
        {
            middleRightSlot.Clear();
            GameMaster.Instance.positions[3] = null;
            middleLeftSlot.Clear();
            GameMaster.Instance.positions[1] = null;
            
        }
        else    //中左、中右雙邊配置
        {
            leftSlot.Clear();
            GameMaster.Instance.positions[0] = null;
            middleSlot.Clear();
            GameMaster.Instance.positions[2] = null;
            rightSlot.Clear();
            GameMaster.Instance.positions[4] = null;
        }
    }
    public void CharacterNoSame(string _name)   //避免同人出現在不同位置
    {
        Sprite targetSprite = imageDic[_name];

        // 用 sprite.name 比對，避免引用地址不同但圖片一樣
        for (int i = 0; i < GameMaster.Instance.positions.Length; i++)
        {
            if (positionSlots.ContainsKey(i))
            {
                var slot = positionSlots[i];
                if (slot.characterImage != null && slot.characterImage.sprite != null && slot.characterImage.sprite.name == targetSprite.name)
                {
                    slot.Clear();
                    GameMaster.Instance.positions[i] = null;
                }
            }
        }
    }

    private void LoadCharacterSprites() //載入所有角色圖片
    {
        Sprite[] characterSprites = Resources.LoadAll<Sprite>("Character");
        foreach (Sprite sprite in characterSprites)
        {
            imageDic[sprite.name] = sprite;
            //Debug.Log($"已載入角色圖：{sprite.name}");
        }
    }
    private void LoadBackgroundSprites()    //載入所有背景圖片
    {
        Sprite[] bgSprites = Resources.LoadAll<Sprite>("Background");
        foreach (Sprite sprite in bgSprites)
        {
            backGroundDic[sprite.name] = sprite;
            //Debug.Log($"已載入背景圖：{sprite.name}");
        }
    }
    public void LoadLoadScene()  //進入存檔介面
    {   
        GameMaster.Instance.isSave = false;
        GameMaster.Instance.LoadSaveLoadScene();
    }
    public void LoadSaveScene()  //進入存檔介面
    { 
        GameMaster.Instance.OnClickSave();
    }
}
