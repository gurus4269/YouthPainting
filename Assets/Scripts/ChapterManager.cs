using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set;} //單例模式
    private string CheckpointFolder => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH_CHECKPOINT);

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

    public void Return()    //返回
    {
        GameMaster.Instance.LoadMenuScene();
    }

    public void ChooseData(int _chapterID)  //進入第x章
    {
        GameMaster.Instance.currentChapterId = _chapterID;
        GameMaster.Instance.LoadGameScene();
   }
}