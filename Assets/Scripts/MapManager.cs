using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] ScenarioPanel;   //每張地圖Panel的陣列
    public GameObject DialogPanel;  //對話panel(GamePanel)
    public GameObject MapPanel;  //大地圖panel
    public int serial;  //畫面序號，對應每個panel
    public static MapManager Instance { get; private set;} //單例模式

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnclickMap()    //顯示Map
    {
        if(MapPanel.activeSelf)
        {
            MapPanel.SetActive(false);
        }
        else
        {
            MapPanel.SetActive(true);
        }
    }
    public void Return()        //取消顯示Map
    {
        MapPanel.SetActive(false);
    }
    
    //進入地圖0 學生會辦
    //進入地圖1 校長室
    //進入地圖2 3-A
    //進入地圖3 走廊A
    //進入地圖4 走廊B
    public void OnclickToMapX(int num)     
    {
        Debug.Log(num);
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = num;
        ScenarioPanel[serial].SetActive(true);
    }

    public void test()
    {
        // 透過指令呼叫對話，指定劇本與起始 ID
        // Dialog.Instance.TriggerDialog(Resources.Load<TextAsset>("Text/YouthPaintingChapter1"), 0);
    }
}
