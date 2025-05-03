using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] characterPanel;     //人物介紹Panel的陣列
    public GameObject All; //主畫面panel
    public int serial;  //畫面序號，對應每個panel

    // Start is called before the first frame update
    void Start()
    {
        serial = -1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnclickIntoRed()
    {
        serial = 0;
        characterPanel[0].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoOrange()
    {
        serial = 1;
        characterPanel[1].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoYellow()
    {
        serial = 2;
        characterPanel[2].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoGreen()
    {
        serial = 3;
        characterPanel[3].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoBlue()
    {
        serial = 4;
        characterPanel[4].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoDarkBlue()
    {
        serial = 5;
        characterPanel[5].SetActive(true);
        All.SetActive(false);
    } 
    public void OnclickIntoPurple()
    {
        serial = 6;
        characterPanel[6].SetActive(true);
        All.SetActive(false);
    } 

    public void OnclickToNext()
    {
        characterPanel[serial].SetActive(false);
        serial = ( serial + 1 ) % 7;
        characterPanel[serial].SetActive(true);
    }
    public void OnclickToPrevious()
    {
        characterPanel[serial].SetActive(false);
        serial = ( serial - 1 ) < 0 ? 6 : ( serial - 1 ) ;
        characterPanel[serial].SetActive(true);
    }
    public void Return()
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
