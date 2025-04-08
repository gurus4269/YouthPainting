using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
//using UnityEngine.UIElements;

public class Dialog : MonoBehaviour
{
    public TextAsset dialogDataFile; //對話文件(.csv)
    public CharacterSlot leftSlot, middleLeftSlot, middleSlot, middleRightSlot, rightSlot;  //角色圖像放的位置
    private Dictionary<int, CharacterSlot> positionSlots = new Dictionary<int, CharacterSlot>();    //角色圖像放的位置字典
    public TMP_Text nameText, dialogText;    //角色名字文件、對話內容文件
    public Button next;     //下一步按鈕
    public GameObject choice;   //選項按鈕預製體
    public Transform choiceGroup;   //放選項的區域
    public BackGroundSlot backgroundSlot;   //背景圖片放置區
    public List<Sprite> sprites = new List<Sprite>();    //角色圖片列表
    Dictionary<string, Sprite> imageDic= new Dictionary<string, Sprite>();  //角色圖片字典
    public List<Sprite> backGroundSprites = new List<Sprite>();    //背景圖片列表
    Dictionary<string, Sprite> backGroundDic= new Dictionary<string, Sprite>();  //背景圖片字典
    private string[] dialogRows;    //按照行分開的文件
    private int dialogIndex = 0;    //當前的對話行數
    


    private void Awake() 
    {
        //人物圖片
        imageDic["Angel"] = sprites[0];
        imageDic["Anna"] = sprites[1];

        //背景圖片
        backGroundDic["A"] = backGroundSprites[0];
        backGroundDic["B"] = backGroundSprites[1];

        //放圖片的位置
        positionSlots[0] = leftSlot;
        positionSlots[1] = middleLeftSlot;
        positionSlots[2] = middleSlot;
        positionSlots[3] = middleRightSlot;
        positionSlots[4] = rightSlot;

    }
    // Start is called before the first frame update
    void Start()
    {
        leftSlot.Clear();   //所有位置的圖片都先設成透明
        middleLeftSlot.Clear();
        middleSlot.Clear();
        middleRightSlot.Clear();
        rightSlot.Clear();

        ReadText(dialogDataFile);
        ShowDialogRow();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateText(string _name, string _text) //更新人名與對話
    {
        nameText.text = _name;  //把人物名字放進 namebar
        dialogText.text = _text;    //把對話放進 dialogbar
    }
    public void UpdateImage(string _name, int _place)   //更新人物立繪
    {
        if (!imageDic.ContainsKey(_name))
        {
            Debug.LogWarning($"角色 {_name} 不存在於 imageDic 中！");
            return;
        }

        ClearCharacter(_place, _name); // 清理其他圖

        // 顯示角色圖片
        if (positionSlots.ContainsKey(_place))
        {
            positionSlots[_place].SetCharacter(imageDic[_name]);
        }
    }

    public void UpdateBackGroundImage(string _bgKey, bool _isMemory = false)    //更新背景
    {
        if (!backGroundDic.ContainsKey(_bgKey))
        {
            Debug.LogWarning($"背景圖片 {_bgKey} 不存在！");
            return;
        }

        backgroundSlot.SetBackground(backGroundDic[_bgKey], _isMemory);
    }

    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');  //把文件分割成行
    }
    public void ShowDialogRow()
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');  //按照逗號分開成每個元素
            if(cells[0] == "#" && int .Parse(cells[1]) == dialogIndex)  //#是一般對話
            {
                UpdateImage(cells[2], int.Parse(cells[3]));     
                UpdateText(cells[2], cells[4]);

                dialogIndex = int.Parse(cells[5]);  //下一行的序號
                break;
            }
            else if(cells[0] == "&" && int .Parse(cells[1]) == dialogIndex) //選項序號
            {
                next.gameObject.SetActive(false);   //關掉下一步按鈕
                GenerateChoice(i);  //生成選項1
            }
            else if (cells[0] == "@" && int.Parse(cells[1]) == dialogIndex) //更改背景
            {
                string bgKey = cells[6];
                bool isMemory =  bool.Parse(cells[7]);
                leftSlot.Clear();
                middleLeftSlot.Clear();
                middleSlot.Clear();
                middleRightSlot.Clear();
                rightSlot.Clear();
                UpdateBackGroundImage(bgKey, isMemory);

                dialogIndex++; // 自動下一行
                ShowDialogRow();
                break;
            }
            else if(cells[0] == "END" && int .Parse(cells[1]) == dialogIndex) //結束
            {
                Debug.Log("劇情結束");
            }
        }
    }
    public void OnClickNext()   //下一步按鈕
    {
        ShowDialogRow();
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
            middleLeftSlot.Clear();
            CharacterNoSame( _name );
            
        }
        else    //中左、中右雙邊配置
        {
            leftSlot.Clear();
            middleSlot.Clear();
            rightSlot.Clear();
            CharacterNoSame( _name );
        }
    }
    public void CharacterNoSame(string _name)
    {
        Sprite targetSprite = imageDic[_name];

        // 用 sprite.name 比對，避免引用地址不同但圖片一樣
        foreach (var slot in positionSlots.Values)
        {
            if (slot.characterImage != null && slot.characterImage.sprite != null &&
                slot.characterImage.sprite.name == targetSprite.name)
            {
                slot.Clear();
            }
        }
    }

}
