using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SettingMenuData", menuName = "SaveData/SettingMenuData")]
public class SettingMenuSaveData : ScriptableObject
{
    [Header("Stats")]
    // 保存したいデータ
    // BGMの音量    BGMVolume = SoundManager.Instance.SettingBGMVolume = InitialSettingBGMVolume
    public float BGMVolume;
    // SEの音量
    public float SEVolume;
    // Darkmodeの On or Off
    public bool IsDarkMode;
    // 言語のType
    // public int LanguageType;
    // enum 表現の
    public UIManager.LanguageType LanguageType;

    [Header("SaveData")]
    public string key;    // Data を保存する時のキー

    // Data の Load (上書き)
    void OnEnable()
    {
        LoadSettingMenuData();

        // 保存されてるデータを他のクラスに設定する
        // SetSaveData();
    }

    // アプリの終了時更新されてる Data を Save(保存)
    void OnDisable()
    {
        // SaveSettingMenuData();
    }

    // SettingMenu の Data を保存
    public void SaveSettingMenuData(float BGMVolume, float SEVolume, bool IsDarkMode, UIManager.LanguageType LanguageType)
    {
        // 保存するData内容の更新
        this.BGMVolume = BGMVolume;
        this.SEVolume = SEVolume;
        this.IsDarkMode = IsDarkMode;
        this.LanguageType = LanguageType;

        // キーの設定
        if(key == "")
        {
            key = name;
        }

        string jsonData = JsonUtility.ToJson(this, true);  // this（このスクリプトの情報） をシリアライズ化(フォーマット化)
        PlayerPrefs.SetString(key, jsonData);              // key に JsonData を格納する
        // PlayerPrefs.Save();                                // 最後に, これらの情報を disc に保存する
        Debug.Log("Save Data");
    }

    // SettingMenu の Data をロード
    public void LoadSettingMenuData()
    {
        // 保存されてる Data を この Data に上書きする
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), this);
        Debug.Log("Data Load");
    }
}
