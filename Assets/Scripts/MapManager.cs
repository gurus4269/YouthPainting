using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] ScenarioPanel;   //每張地圖Panel的陣列
    public GameObject DialogPanel;  //對話panel(GamePanel)
    public GameObject[] MapPanel;  //地圖panel陣列  0 => 大地圖、1 => 學校、2 => 宿舍
    public int serial;  //畫面序號，對應每個panel
    public int mapNum = -1;  //0 => 大地圖、1 => 學校、2 => 宿舍
    public static MapManager Instance { get; private set; } //單例模式

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
        serial = 1;
        OnclickMap(-1);
        OnclickToMapX(13);
        print("A");
    }

    public void OnclickMap(int num)    //顯示Map
    {
        mapNum = num;
        for (int i = 0; i < MapPanel.Length; i++)
        {
            if (i == mapNum)
            {
                MapPanel[i].SetActive(true);
            }
            else
            {
                MapPanel[i].SetActive(false);
            }
        }
    }
    public void Return()        //取消顯示Map
    {
        if (mapNum != 0)
        {
            mapNum = 0;
            OnclickMap(mapNum);
        }
        else
        {
            mapNum = -1;
            OnclickMap(mapNum);
        }
    }

    //進入地圖0 3-A
    //進入地圖1 3-B
    //進入地圖2 3-C
    //進入地圖3 3-D
    //進入地圖4 走廊
    //進入地圖5 教師辦公室
    //進入地圖6 社團教室
    //進入地圖7 澪瑠的房間
    //進入地圖8 芙玖的房間
    //進入地圖9 霞乃的房間
    //進入地圖10 柚希的房間
    //進入地圖11 蒼依的房間
    //進入地圖12 宿舍走廊
    //進入地圖13 禮堂
    public void OnclickToMapX(int num)
    {
        Debug.Log(num);
        ScenarioPanel[serial].SetActive(false);
        mapNum = -1;
        OnclickMap(mapNum);
        serial = num;
        ScenarioPanel[serial].SetActive(true);
    }

    public void test()
    {
        // 透過指令呼叫對話，指定劇本與起始 ID
        // Dialog.Instance.TriggerDialog(Resources.Load<TextAsset>("Text/YouthPaintingChapter1"), 0);
    }
}
