using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    //public List<GameObject> mode = new List<GameObject>();  //可替換畫面
    public static EndManager Instance { get; private set; } //單例模式

    private void Awake()
    {
        if (Instance == null)    //刪除多的單例，如果沒有單例，則自己成為單例
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnclickHome()   //返回開始畫面
    {
        GameMaster.Instance.LoadMenuScene();
    }
    public void OnclickStage()
    {
        GameMaster.Instance.LoadChapterScene();
    }
}
