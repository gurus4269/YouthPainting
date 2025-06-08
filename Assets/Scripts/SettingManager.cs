using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Toggle fullScreenToggle;     //全螢幕 or 視窗
    public Text toggleLable;            //開關顯示的內容
    public TMP_Dropdown resolutionDropdown; //分辨率

    private Resolution[] availableResolutions;
    private Resolution defaultResolution;
    public Button defaultButton;


    public static SettingManager Instance { get; private set; } //單例模式

    private void Awake()
    {
        if (Instance == null)
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
        InitializeResolutions();
        fullScreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        UpdateToggleLabel(fullScreenToggle.isOn);

        fullScreenToggle.onValueChanged.AddListener(SetDisplayMode);
        resolutionDropdown.onValueChanged.AddListener(SetReolution);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickReturn()     //返回按鈕事件
    {
        GameMaster.Instance.LoadMenuScene();
    }
    void InitializeResolutions()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var resolutionMap = new Dictionary<string, Resolution>();
        int currentResolutionIndex = 0;

        foreach (var res in availableResolutions)
        {
            const float aspectRaito = 16f / 9f;
            const float epsilon = 0.01f;

            if (Mathf.Abs((float)res.width / res.height - aspectRaito) > epsilon)
                continue;

            string option = res.width + "x" + res.height;
            if (!resolutionMap.ContainsKey(option))
            {
                resolutionMap[option] = res;
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));

                if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = resolutionDropdown.options.Count - 1;
                    defaultResolution = res;
                }
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    void UpdateToggleLabel(bool isFullscreen)
    {
        toggleLable.text = isFullscreen ? "Fullscreen" : "Windowed";
    }

    void SetDisplayMode(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        UpdateToggleLabel(isFullscreen);
    }

    void SetReolution(int index)
    {
        string[] dimensions = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(dimensions[0].Trim());
        int height = int.Parse(dimensions[1].Trim());
        Screen.SetResolution(width, height, Screen.fullScreenMode);

    }

    // void SaveSettings()
    // {
    //     PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
    //     PlayerPrefs.SetInt("Fullscreen", fullScreenToggle.isOn ? 1 : 0);
    //     PlayerPrefs.Save();
    // }

    public void ResetSetting()
    {
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(
            Option => Option.text == $"{defaultResolution.width}x{defaultResolution.height}");
        fullScreenToggle.isOn = true;
    }
}
