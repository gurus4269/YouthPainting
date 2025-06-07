using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterPanel;     //人物介紹Panel的陣列
    public GameObject All; //主畫面panel
    public int serial;  //畫面序號，對應每個panel
    public static CharacterManager Instance { get; private set;} //單例模式

    private void Awake()
    {
        if(Instance == null)    //刪除多的單例，如果沒有單例，則自己成為單例
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
        serial = -1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //進入角色x  0 => 澪瑠, 
    //          1 => 芙玖, 
    //          2 => 蒼依, 
    //          3 => 柚希, 
    //          4 => 霞乃, 
    public void OnclickIntoCharacter(int num)   
    {
        serial = num;
        characterPanel[serial].SetActive(true);
        All.SetActive(false);
    } 

    public void OnclickToNext() //下一個角色
    {
        characterPanel[serial].SetActive(false);
        serial = ( serial + 1 ) % 5;
        characterPanel[serial].SetActive(true);
    }
    public void OnclickToPrevious() //上一個角色
    {
        characterPanel[serial].SetActive(false);
        serial = ( serial - 1 ) < 0 ? 4 : ( serial - 1 ) ;
        characterPanel[serial].SetActive(true);
    }
    public void Return()    //上一頁
    {
        if(serial == -1)
        {
            GameMaster.Instance.LoadMenuScene();
        }
        else
        {
            characterPanel[serial].SetActive(false);
            serial = -1;
            All.SetActive(true);
        }
    }
}
