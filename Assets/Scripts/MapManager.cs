using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] ScenarioPanel;   //地圖Panel的陣列
    public GameObject DialogPanel;  //對話panel
    public GameObject MapPanel;  //對話panel
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
        serial = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnclickMap()    //顯示Map
    {
        MapPanel.SetActive(true);
    }
    public void Return()        //取消顯示Map
    {
        MapPanel.SetActive(false);
    }
    public void OnclickToMap0()     //進入地圖0 學生會辦
    {
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = 0;
        ScenarioPanel[serial].SetActive(true);
    }
    public void OnclickToMap1()     //進入地圖1 校長室
    {
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = 1;
        ScenarioPanel[serial].SetActive(true);
    }
    public void OnclickToMap2()     //進入地圖2 3-A
    {
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = 2;
        ScenarioPanel[serial].SetActive(true);
    }
    public void OnclickToMap3()     //進入地圖3 走廊A
    {
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = 3;
        ScenarioPanel[serial].SetActive(true);
    }
    public void OnclickToMap4()     //進入地圖4 走廊B
    {
        ScenarioPanel[serial].SetActive(false);
        MapPanel.SetActive(false);
        serial = 4;
        ScenarioPanel[serial].SetActive(true);
    }
    public void test()
    {
        // 透過指令呼叫對話，指定劇本與起始 ID
        Dialog.Instance.TriggerDialog(Resources.Load<TextAsset>("Text/YouthPaintingChapter1"), 0);
    }
}
