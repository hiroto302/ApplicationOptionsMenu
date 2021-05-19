using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SettingMenu を統括するクラス
public class SettingMenu : MonoBehaviour
{
    [SerializeField] BGMVolumeSlider _bgmVolumeSlider;  // BGM に音量を扱う
    [SerializeField] SaveButton _saveButton;

    // SettingMenu の値が変更後、Save されずにMenuが閉ざされた時、
    // 再度初期値に戻すために初期値をシステムクラスから得て変更する
    void OnEnable()
    {
        InitializeSettings();
    }

    void Start()
    {
        _saveButton.OnSettingValueSave.AddListener(OnHandleSaveButton);  // event 処理追加
    }

    // Save ボタンが押された時の処理
    void OnHandleSaveButton()
    {
        SoundManager.Instance.ChangeBGMVolume(_bgmVolumeSlider.BGMVolume);
    }

    // SettingMenuの 初期化
    void InitializeSettings()
    {
        _bgmVolumeSlider.gameObject.GetComponent<Slider>().value = SoundManager.Instance.SettingBGMVolume;
    }

}