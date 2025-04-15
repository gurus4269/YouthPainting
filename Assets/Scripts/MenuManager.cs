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

    
    public void StartGame()//開始新遊戲
    {
        GameMaster.Instance.hasStarted = true; 
        GameMaster.Instance.historyList.Clear();
        GameMaster.Instance.LoadGameScene(0);
        
    }

    public void ContinueGame()  //回到遊戲(繼續)
    {
        if(GameMaster.Instance.hasStarted == true)  //若沒有開始過，則禁止進入
        {
            GameMaster.Instance.LoadGameScene(GameMaster.Instance.currentSpeakingContentID) ;
        }
    }
    public void LoadGame()  //跳轉到SaveLoadScene
    {   
        GameMaster.Instance.hasStarted = true;
        GameMaster.Instance.isSave = false;
        GameMaster.Instance.LoadSaveLoadScene();
    }
    public void Setting()  //跳轉到SaveLoadScene
    {   
        GameMaster.Instance.LoadSettingScene();
    }
    public void Gallery()  //跳轉到GalleryScene
    {   
        GameMaster.Instance.LoadGalleryScene();
    }
}
