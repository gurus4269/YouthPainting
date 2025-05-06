using UnityEngine;

public class ScreenShotter : MonoBehaviour
{
    //可刪掉
    public Texture2D CaptureScreenShot()    //處理拍攝
    {
        int width = Screen.width;   //設定寬高
        int height = Screen.height;

        RenderTexture rt = RenderTexture.GetTemporary(width, height, 24);

        Camera mainCamera = Camera.main;

        if(mainCamera == null)
        {
            Debug.LogError("camera not found");
        }

        mainCamera.targetTexture = rt;
        RenderTexture.active = rt;
        mainCamera.Render();

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24,false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        Texture2D resizedScreenShot  = ResizeTexture(screenshot, width / 4, height / 4);

        return resizedScreenShot;
    }

    private Texture2D ResizeTexture(Texture2D original, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight, 24);
        RenderTexture.active = rt;

        Graphics.Blit(original, rt);

        Texture2D resized = new Texture2D(newWidth, newHeight, TextureFormat.RGB24,false);
        resized.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        resized.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return resized;
    }

}
