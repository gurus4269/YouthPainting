using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
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
}
