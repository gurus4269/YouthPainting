using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    public static GalleryManager Instance { get; private set;} //單例模式

    private void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickReturn()   //返回開始畫面
    {
        GameMaster.Instance.LoadMenuScene();
    }
}
