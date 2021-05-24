using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField] SettingMenuSaveData _settingMenuSaveData;

    // Awake の次に実行される関数
    void OnEnable()
    {
        // データ内容を更新
        SetSettingMenuSaveData();
    }

    void OnDisable()
    {
        /* このタイミングで呼ぶと他のクラスの情報を参照することができない可能性があるので
            SaveButtonが押されたタイミングで内容を保存したいDataを反映させる */
        // SaveSettingMenuData();
    }

    void Update()
    {
        // Debug用
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveSettingMenuData();
        }

        // 保存されてるDataをロードし、他のクラスに内容を反映
        if(Input.GetKeyDown(KeyCode.L))
        {
            _settingMenuSaveData.LoadSettingMenuData();
            SetSettingMenuSaveData();
            Debug.Log("Load");
        }
    }

    // 設定されてる内容の保存
    public void SaveSettingMenuData()
    {
        // float bgmVolume = SoundManager.Instance.SettingBGMVolume;
        float bgmVolume = SoundManager.Instance.SettingBGMVolume;
        float seVolume = SoundManager.Instance.SettingSEVolume;
        bool isDarkmode = UIManager.Instance.IsDarkmode;


        _settingMenuSaveData.SaveSettingMenuData(bgmVolume, seVolume, isDarkmode);
        Debug.Log("Save");
    }

    // 保存されていた SettingMenu の Data を各クラスに代入
    void SetSettingMenuSaveData()
    {
        //  Dataが保存されてる時, event で各 manager クラスに知らせる or 直接値を代入する
        //  とりあえず、値を代入していく
        if(_settingMenuSaveData.key != "")
        {
            SoundManager.Instance.InitialBGMVolume = _settingMenuSaveData.BGMVolume;
            SoundManager.Instance.InitialSEVolume = _settingMenuSaveData.SEVolume;
            UIManager.Instance.IsDarkmode = _settingMenuSaveData.IsDarkMode;

            // if(_settingMenuSaveData.LanguageType == 0)
            // {
            //     UIManager.Instance.ChangeLanguageType(UIManager.LanguageType.English);
            // }
            // else if(_settingMenuSaveData.LanguageType == 1)
            // {
            //     UIManager.Instance.ChangeLanguageType(UIManager.LanguageType.Japanese);
            // }
        }
    }
}
