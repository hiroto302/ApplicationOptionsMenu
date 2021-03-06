using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SettingMenu を統括するクラス
public class SettingMenu : MonoBehaviour
{
    [SerializeField] BGMVolumeSlider _bgmVolumeSlider;  // BGM の音量を扱う
    [SerializeField] SEVolumeSlider _seVolumeSlider;    // SE の音量を扱う
    [SerializeField] DarkmodeToggle _darkmodeToggle;    // Darkmode の切り替えを扱う
    [SerializeField] LanguageSelectionDropdown _languageSelectionDropdown;  // 言語の切り替えを扱う
    [SerializeField] SaveButton _saveButton;            // Saveを実行するボタン

    // 初期化する時の delegate event
    public  delegate void Initialization();
    public static event Initialization InitializeSettings;


    // SettingMenu の値が変更後、Save されずにMenuが閉ざされた時、
    // 再度初期値に戻すために初期値をシステムクラスから得て変更する
    void OnEnable()
    {
        if(InitializeSettings != null)
        {
            // 初期処理を行う event 実行
            InitializeSettings();
        }
    }

    void Start()
    {
        _saveButton.OnSettingValueSave.AddListener(OnHandleSaveButton);  // event 処理追加
    }

    // Save ボタンが押された時の処理
    void OnHandleSaveButton()
    {
        SoundManager.Instance.ChangeBGMVolume(_bgmVolumeSlider.BGMVolume);  // BGMVolume 設定変更
        SoundManager.Instance.ChangeSEVolume(_seVolumeSlider.SEVolume);     // SEVolume 設定変更
        UIManager.Instance.IsDarkmode =  _darkmodeToggle.darkmode;          // Darkmode 設定変更
        UIManager.Instance.ChangeLanguageType(_languageSelectionDropdown.SelectedLanguageType()); // 表示言語 設定変更

        DataManager.Instance.SaveSettingMenuData();     // 変更された内容を保存
    }
}