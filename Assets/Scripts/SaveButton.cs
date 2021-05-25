using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SaveButtonが押された時, Setting Menuで設定した内容を保存し、変更する仕組みで実装
public class SaveButton : MonoBehaviour
{
    [SerializeField] Button saveButton;

    public Events.EventSaveSetting OnSettingValueSave;

    void Start()
    {
        saveButton.onClick.AddListener(SaveSetting);
    }

    // ボタンが押され時に変更内容を反映すること、SettingMenu に知らせる
    public void SaveSetting()
    {
        OnSettingValueSave.Invoke();
    }
}
