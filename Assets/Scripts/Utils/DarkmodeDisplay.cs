using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UIManager が Darkmode に切り替わった時、Darkmodeの切り替えを行いたい Text, 背景画像 を含む親クラスにアタッチして使用するクラス
// 変更したい 背景画像に関しては UIBackground に Tag を変更すること
public class DarkmodeDisplay : MonoBehaviour
{
    void OnEnable()
    {
        ChangeDarkModeDisplay(UIManager.Instance.IsDarkmode, this.gameObject);
    }

    void Start()
    {
        UIManager.Instance.OnDarkmodeChange.AddListener(HandleOnDarkmodeChange);
    }

    // Darkmode の設定が変更された時に実行する処理
    void HandleOnDarkmodeChange(bool darkmode)
    {
        ChangeDarkModeDisplay(darkmode, this.gameObject);
    }

    // Darkmode を実行する処理 : 第一引数 darkモードにするか, 第二引数 Canvasを含む親クラス
    public void ChangeDarkModeDisplay(bool darkmode, GameObject rootObject)
    {
        // SaveButtonが押された時,DarkMode が切り替わってる時のみ処理を実行
        ChangeAllTextColor(darkmode, rootObject);
        ChangeAllImageColor(darkmode, rootObject);
    }

    // Text の 色を切り替える
    public void ChangeAllTextColor(bool darkmode, GameObject rootObject)
    {
        Color textColor = darkmode ? Color.white : Color.black;
        Text[] allText = rootObject.GetComponentsInChildren<Text>(true);
        foreach(var text in allText)
        {
            text.color = textColor;
        }
    }
    // Text を表示してる 背景画像の色を切り替える
    public void ChangeAllImageColor(bool darkmode, GameObject rootObject)
    {
        Color imageColor = darkmode ? new Color32(0, 0, 0, 140) : new Color32(255, 255, 255, 140);
        Image[] allImages = rootObject.GetComponentsInChildren<Image>(true);
        foreach(var image in allImages)
        {
            if(image.gameObject.CompareTag("UIBackground"))
            {
                image.color = imageColor;
            }
        }
    }
}

