using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// SaveButtonが押された時, Setting Menuで設定した内容を保存し、変更する仕組みにする
// まずはじめに、ボタンが押され時に変更内容が反映されるようにする
public class SaveButton : MonoBehaviour
{
    [SerializeField] Button saveButton;

    public Events.EventSaveSetting OnSettingValueSave;

    void Start()
    {
        saveButton.onClick.AddListener(SaveSetting);
    }

    public void SaveSetting()
    {
        OnSettingValueSave.Invoke();
    }
}
