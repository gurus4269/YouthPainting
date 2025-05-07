using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button newGameButton;    //New Game按鈕
    public Button continueButton;   //Continue按鈕
    public Button loadGameButton;   //Load Game按鈕
    public Button settingButton;    //Setting按鈕
    public Button exitButton;       //Exit按鈕

    public static MenuManager Instance { get; private set;} //單例模式

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

    
    public void StartGame()//開始新遊戲 **待修改
    {
        GameMaster.Instance.historyList.Clear();
        GameMaster.Instance.LoadGameScene(0);
        
    }
    public void Chapter()
    {
        GameMaster.Instance.LoadChapterScene();
    }
    public void Setting()  //跳轉到SaveLoadScene
    {   
        GameMaster.Instance.LoadSettingScene();
    }
    public void Gallery()  //跳轉到GalleryScene
    {   
        GameMaster.Instance.LoadGalleryScene();
    }
    public void Character()  //跳轉到CharacterScene
    {   
        GameMaster.Instance.LoadCharacterScene();
    }
}
