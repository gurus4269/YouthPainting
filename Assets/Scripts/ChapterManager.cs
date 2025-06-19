using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set; } //單例模式
    //private string CheckpointFolder => Path.Combine(Application.persistentDataPath, Constants.SAVE_FILE_PATH_CHECKPOINT);
    private Dictionary<int, ChapterData> chapters = new Dictionary<int, ChapterData>();   //event字典
    public List<GameObject> lockList = new List<GameObject>();   //每個章節的鎖
    public List<Button> chapterButton = new List<Button>();     //每個章節按鈕

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
        LoadChapters();     // 載入章節存檔資料
        CheckChapter();     // 檢查lock
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
        Constants.CURRENT_CHPATER_ID = _chapterID;
        GameMaster.Instance.LoadGameScene();
    }

    private string GetFolder()  //放chapterCheck的資料夾路徑(在存檔處)
    {
        return Path.Combine(
            Application.persistentDataPath,
            $"{Constants.CHAPTER_CHECK}"
        );
    }
    private string GetChapterCheckPath()    //存在Assset中的chapterCheck檔
    {
        return Path.Combine(
            Application.streamingAssetsPath,
            $"{Constants.CHAPTER_CHECK}"
        );
    }
    void LoadChapters()   // 載入章節資料
    {
        string chapterChecks = GetFolder();
        if (!File.Exists(chapterChecks))    //檢查檔案是否存在
        {
            string streamingAssets = GetChapterCheckPath();

            Directory.CreateDirectory(Path.GetDirectoryName(chapterChecks));
            File.Copy(streamingAssets, chapterChecks);

            //return;
        }

        string json = File.ReadAllText(chapterChecks);              //讀檔
        ChapterList list = JsonUtility.FromJson<ChapterList>(json);     //
        foreach (var c in list.chapters)                              //放入字典
        {
            chapters[c.Index] = c;
        }
    }

    public void CheckChapter()  //如果尚未開放，則設為設為不可點擊;若開放，則隱藏鎖
    {
        foreach (var c in chapters.Values)
        {
            if (c.Lock == false)
            {
                lockList[c.Index].SetActive(false);
            }
            else
            {
                chapterButton[c.Index].interactable = false;
            }
        }
    }
    public void unlock(int chapter) //解鎖下一章
    {
        chapters[chapter].Lock = false;


        string chapteraddress = GetFolder();
        ChapterList chapterList = new ChapterList
        {
            chapters = chapters.Values.ToList()
        };
        string eventJson = JsonUtility.ToJson(chapterList, true); // 排版美化
        File.WriteAllText(chapteraddress, eventJson);
        Debug.Log($"事件已儲存到：{chapteraddress}");
    }

    public void ComingSoon()    //跳到ComingSoon頁面
    {
        GameMaster.Instance.LoadComingSoonScene();
    }
}