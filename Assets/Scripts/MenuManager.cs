using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;    //開始頁面
    public Button newGameButton;    //New Game按鈕
    public Button continueButton;   //Continue按鈕
    public Button loadGameButton;   //Load Game按鈕
    public Button settingButton;    //Setting按鈕
    public Button exitButton;       //Exit按鈕
    private bool hasStarted = false;    //判斷是否已經開始(1)，未開始(0)
    
    // Start is called before the first frame update
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
        hasStarted = true;
        Dialog.Instance.gamePanel.SetActive(true);
        Dialog.Instance.StartGame();
        menuPanel.SetActive(false);
        
    }

    public void ContinueGame()  //回到遊戲(繼續)
    {
        if(hasStarted)
        {
            menuPanel.SetActive(false);
            Dialog.Instance.gamePanel.SetActive(true);
        }
    }
    public void LoadGame()
    {
        hasStarted = true;
        SaveLoadManager.Instance.OnClickLoad();
    }
}
