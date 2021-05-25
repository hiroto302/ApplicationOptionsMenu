using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] SettingMenuSaveData _settingMenuSaveData;

    // Awake の次に実行される関数
    void OnEnable()
    {
        // データ内容を保存されてるデータに更新
        SetSettingMenuSaveData();
    }

    void OnDisable()
    {
        /* SaveSettingMenuData()をこのタイミングで実行すると、他のクラスの情報を参照することができない可能性があるので
            SaveButtonが押されたタイミングで内容を保存したいDataを反映させる */
    }

    // 設定されてる内容の保存
    public void SaveSettingMenuData()
    {
        // 保存するDataの取得
        float bgmVolume = SoundManager.Instance.SettingBGMVolume;
        float seVolume = SoundManager.Instance.SettingSEVolume;
        bool isDarkmode = UIManager.Instance.IsDarkmode;
        UIManager.LanguageType languageType = UIManager.Instance.CurrentLanguageType;

        // Data の保存
        _settingMenuSaveData.SaveSettingMenuData(bgmVolume, seVolume, isDarkmode, languageType);
    }

    // 保存されている SettingMenu の Data を各クラスに代入
    void SetSettingMenuSaveData()
    {
        //  Dataが保存されてる時, event で各 manager クラスに知らせる or 直接値を代入する. 今回は値を代入
        if(_settingMenuSaveData.key != "")
        {
            SoundManager.Instance.InitialBGMVolume = _settingMenuSaveData.BGMVolume;
            SoundManager.Instance.InitialSEVolume = _settingMenuSaveData.SEVolume;
            UIManager.Instance.IsDarkmode = _settingMenuSaveData.IsDarkMode;
            UIManager.Instance.CurrentLanguageType = _settingMenuSaveData.LanguageType;
        }
    }
}
