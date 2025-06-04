using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;
using System.Collections;
//using UnityEngine.UIElements;

public class Dialog : MonoBehaviour
{
    public CharacterSlot leftSlot, middleLeftSlot, middleSlot, middleRightSlot, rightSlot;  //角色圖像放的位置
    private Dictionary<int, CharacterSlot> positionSlots = new Dictionary<int, CharacterSlot>();    //角色圖像放的位置字典
    public TMP_Text nameText, dialogText;    //對話框中的角色名字欄位、對話內容欄位
    public Button next;     //下一步按鈕
    public GameObject choice;   //選項按鈕預製體
    public Transform choiceGroup;   //放選項的區域
    public GameObject historyContent;   //歷史對話預製體
    public Transform historyContentGroup;   //放歷史對話預製體的地方
    public BackGroundSlot backgroundSlot;   //背景圖片放置區
    public GameObject settingUI, textUI;    //側邊按鈕, 下方對話框
    public Button autoPlay, speedPlay;      //自動撥放按鈕、加速按鈕
    public int dialogIndex = Constants.DIALOGINDEX;    //當前的對話行數

    public GameObject gamePanel;    //遊戲畫面
    public GameObject historyPanel; //歷史對話紀錄畫面
    public float autoPlaySpeed = Constants.DIALOGSPEEDSLOW;    //設定自動播放速度
    public bool isAutoPlaying = false;  //是否在自動播放
    public bool isSpeedUp = false;  //是否在自動播放
    private Coroutine autoPlayCoroutine;
    
    public List<Sprite> sprites = new List<Sprite>();    //角色圖片列表
    private Dictionary<string, Sprite> imageDic= new Dictionary<string, Sprite>();  //角色圖片字典
    public List<Sprite> backGroundSprites = new List<Sprite>();    //背景圖片列表
    private Dictionary<string, Sprite> backGroundDic= new Dictionary<string, Sprite>();  //背景圖片字典
    private string[] dialogRows;    //按照行分開的文件

    public static Dialog Instance { get; private set;} //單例模式

    private void Awake() 
    {
        
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
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //點擊事件：如果現在TextUI以及Setting被關掉，則打開他們
        {
            OpenUI();
        }
    }

    public void TriggerDialog(TextAsset dialogFile, int startIndex) //用來觸發的(劇本名，開始序號)
    {
        gamePanel.SetActive(true);  //顯示對話框
        leftSlot.Clear();           //清空人物框框
        middleLeftSlot.Clear();
        middleSlot.Clear();
        middleRightSlot.Clear();
        rightSlot.Clear();

        ReadText(dialogFile);       //讀劇本
        dialogIndex = startIndex;   //設定開始序號
        ShowDialogRow();            //讀一行
        
    }
    public void UpdateText(string _name, string _text) //更新人名與對話
    {
        nameText.text = _name;  //把人物名字放進 namebar
        dialogText.text = _text;    //把對話放進 dialogbar
    }
    public void UpdateTextOnly(string _text) //只更新對話
    {
        dialogText.text = _text;    //把對話放進 dialogbar
    }
    public void UpdateImage(string _name, int _place)   //更新人物立繪
    {   
        if(_place != 5)
        {
            if (!imageDic.ContainsKey(_name))   //防呆
            {
                Debug.LogWarning($"角色 {_name} 不存在於 imageDic 中！");
                return;
            }

            ClearCharacter(_place, _name);  // 清理不同配置的圖
            CharacterNoSame( _name );       //不同位置的相同圖

            // 顯示角色圖片
            if (positionSlots.ContainsKey(_place))
            {
                positionSlots[_place].SetCharacter(imageDic[_name]);
            }
        }
        
    }

    public void UpdateBackGroundImage(string _bgKey, int _isMemory = 0)    //更新背景
    {
        if (!backGroundDic.ContainsKey(_bgKey))     //防呆
        {
            Debug.LogWarning($"背景圖片 {_bgKey} 不存在！");
            backgroundSlot.SetBackground(backGroundDic["title2"], _isMemory); //更新背景
            return;
        }

        backgroundSlot.SetBackground(backGroundDic[_bgKey], _isMemory); //更新背景
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
                
                //歷史紀錄用
                if(GameMaster.Instance.historyList.Count > Constants.HISTORYLINES)  //如果裡面行數大於設定的數
                {
                    GameMaster.Instance.historyList.RemoveAt(0);    //移除最前面多餘的
                }
                GameMaster.Instance.historyList.Add(cells[4]);
                if(GameMaster.Instance.historyList.Count >= 2)
                {
                    if(GameMaster.Instance.historyList[^1] == GameMaster.Instance.historyList[^2])
                    {
                        GameMaster.Instance.historyList.RemoveAt(GameMaster.Instance.historyList.Count - 1);  //新增歷史紀錄到list
                    }
                }
                
                // GameObject history = Instantiate(historyContent, historyContentGroup);   //將歷史紀錄預製體加入到群組中
                // history.GetComponent<TMP_Text>().text = cells[4];   //給選項添加文字

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
                break;
            }
            else if(cells[0] == Constants.CHOICE && int .Parse(cells[1]) == dialogIndex) //選項
            {
                autoPlay.interactable = false;
                speedPlay.interactable = false;
                next.gameObject.SetActive(false);   //關掉下一步按鈕
                GenerateChoice(i);  //生成選項1
                if(isAutoPlaying)   //若正在自動播放，則停止
                {
                    OnclickAutoPlay(); // 自動關閉
                }
            }
            else if(cells[0] == Constants.BACKGROUND && int.Parse(cells[1]) == dialogIndex) //更改背景
            {
                string bgKey = cells[2];    //存人物
                int isMemory =  int.Parse(cells[3]);  //存位置

                leftSlot.Clear();       //清空所有位置上的人
                middleLeftSlot.Clear();
                middleSlot.Clear();
                middleRightSlot.Clear();
                rightSlot.Clear();

                UpdateBackGroundImage(bgKey, isMemory);

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
            }
            else if(cells[0] == Constants.END && int .Parse(cells[1]) == dialogIndex) //結束
            {
                Debug.Log("劇情結束");
                if(isAutoPlaying)   //若正在自動播放，則停止
                {
                    OnclickAutoPlay(); // 自動關閉
                }
                
                gamePanel.SetActive(false);
                if (cells[5] != null)
                {
                    EventManager.Instance.CheckOpen(int.Parse(cells[5]));
                }
                //GameMaster.Instance.LoadEndScene();
            }
            else if(cells[0] == Constants.WAIT && int .Parse(cells[1]) == dialogIndex) //$等待
            {
                Debug.Log("實現等待功能");
                dialogIndex = int.Parse(cells[5]);  //下一行的序號
                break;
            }
            else if(cells[0] == Constants.NARRATION && int .Parse(cells[1]) == dialogIndex) //旁白
            {
                UpdateTextOnly(cells[4]);

                //歷史紀錄用
                if(GameMaster.Instance.historyList.Count > Constants.HISTORYLINES)  //如果裡面行數大於設定的數
                {
                    GameMaster.Instance.historyList.RemoveAt(0);    //移除最前面多餘的
                }
                GameMaster.Instance.historyList.Add(cells[4]);
                if(GameMaster.Instance.historyList.Count >= 2)
                {
                    if(GameMaster.Instance.historyList[^1] == GameMaster.Instance.historyList[^2])
                    {
                        GameMaster.Instance.historyList.RemoveAt(GameMaster.Instance.historyList.Count - 1);  //新增歷史紀錄到list
                    }
                }

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
                break;
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
                        OnChoiceClick(int.Parse(cells[5]), cells[4]);
                    } 
                );    
            GenerateChoice( _index + 1 );
        }
        
    }
    public void OnChoiceClick(int _id, string _content)  //選項觸發事件(跳到回答的那行)
    {
        //歷史紀錄部分
        if (GameMaster.Instance.historyList.Count > Constants.HISTORYLINES)  //如果裡面行數大於設定的數
        {
            GameMaster.Instance.historyList.RemoveAt(0);    //移除最前面多餘的
        }
        GameMaster.Instance.historyList.Add(_content);  //新增歷史紀錄到list
        // GameObject history = Instantiate(historyContent, historyContentGroup);   //將歷史紀錄預製體加入到群組中
        // history.GetComponent<TMP_Text>().text = _content;   //給選項添加文字

        dialogIndex = _id;
        ShowDialogRow();
        for (int i = 0; i < choiceGroup.childCount; i++)   //消除選項
        {
            Destroy(choiceGroup.GetChild(i).gameObject);
        }
        next.gameObject.SetActive(true);
        autoPlay.interactable = true;
        speedPlay.interactable = true;

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
            middleLeftSlot.Clear();
        }
        else    //中左、中右雙邊配置
        {
            leftSlot.Clear();
            middleSlot.Clear();
            rightSlot.Clear();
        }
    }
    public void CharacterNoSame(string _name)   //避免同人出現在不同位置
    {
        Sprite targetSprite = imageDic[_name];

        // 用 sprite.name 比對，避免引用地址不同但圖片一樣
        for (int i = 0; i < 5; i++)
        {
            if (positionSlots.ContainsKey(i))
            {
                var slot = positionSlots[i];
                if (slot.characterImage != null && slot.characterImage.sprite != null && slot.characterImage.sprite.name == targetSprite.name)
                {
                    slot.Clear();
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
    // public void EnterLoadScene()  //進入存檔介面
    // {   
    //     GameMaster.Instance.isSave = false;
    //     GameMaster.Instance.LoadSaveLoadScene();
    // }
    // public void EnterSaveScene()  //進入存檔介面
    // { 
    //     GameMaster.Instance.OnClickSave();
    // }
    public void EnterHistoryPanel()  //進入History Panel
    { 
        //gamePanel.SetActive(false);
        for (int i = 0; i < historyContentGroup.childCount ; i++)   //消除原紀錄
        {
            Destroy(historyContentGroup.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameMaster.Instance.historyList.Count; i++)
        {
            GameObject history = Instantiate(historyContent, historyContentGroup);   //將歷史紀錄預製體加入到群組中
            history.GetComponent<TMP_Text>().text = GameMaster.Instance.historyList[i];   //給選項添加文字
        }
        historyPanel.SetActive(true);

    }
    public void BackToGameScene()  //返回DialogPanel
    { 
        historyPanel.SetActive(false);
        //gamePanel.SetActive(true);
    }
    public void OnclickAutoPlay()  // 點擊自動播放按鈕
    {
        if (!isAutoPlaying) //開啟自動播放
        {
            isSpeedUp = false;
            autoPlaySpeed = Constants.DIALOGSPEEDSLOW;
            isAutoPlaying = true;   //開啟自動播放
            next.gameObject.SetActive(false);   //關閉下一步按鈕
            autoPlayCoroutine = StartCoroutine(AutoPlayRoutine());  //啟動自動播放
        }
        else    //關閉自動播放
        {
            isSpeedUp = false;
            autoPlaySpeed = Constants.DIALOGSPEEDSLOW;
            isAutoPlaying = false;  //關閉自動播放
            next.gameObject.SetActive(true);    //關閉下一步按鈕
            if (autoPlayCoroutine != null)      //若自動播放協程存在
            {
                StopCoroutine(autoPlayCoroutine);   //關閉
                autoPlayCoroutine = null;           //刪除
            }
        }
        
    }
    public void OnclickSpeedUp()    //點擊加速按鈕
    {
        if (isAutoPlaying)
        {
            if (isSpeedUp)
            {
                isSpeedUp = false;
                autoPlaySpeed = Constants.DIALOGSPEEDSLOW;
                Debug.Log("Speed down");
            }
            else
            {
                isSpeedUp = true;
                autoPlaySpeed = Constants.DIALOGSPEEDFAST;
                Debug.Log("Speed up");
            }
        }
        else
        {
            isSpeedUp = true;
            autoPlaySpeed = Constants.DIALOGSPEEDFAST;
            isAutoPlaying = true;   //開啟自動播放
            next.gameObject.SetActive(false);   //關閉下一步按鈕
            autoPlayCoroutine = StartCoroutine(AutoPlayRoutine());  //啟動自動播放
        }
    }
    private IEnumerator AutoPlayRoutine()   //自動撥放協程
    {
        while (true)
        {
            ShowDialogRow();    //顯示下一行

            yield return new WaitForSeconds(autoPlaySpeed);
        }
    }
}
