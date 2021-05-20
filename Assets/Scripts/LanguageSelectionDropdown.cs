using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 表示言語の選択するドロップダウンボタン
public class LanguageSelectionDropdown : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    int _dropdownIndex; // 現在選択されてる 項目の value の値
    UIManager.LanguageType languageType;  // 選択されている項目に対応している 言語の種類

    void Start()
    {
        InitializeSetting();
        dropdown.onValueChanged.AddListener(HandleLanguageSelectionDropdown);
        SettingMenu.InitializeSettings += InitializeSetting;
    }

    // Dropwon 選択した時発生する event に対応した処理. value は現在選択されてる option の index が返ってくる。
    void HandleLanguageSelectionDropdown(int value)
    {
        _dropdownIndex = value;
    }

    // 選択されてる項目に対応する言語の種類を返すメソッド
    public UIManager.LanguageType SelectedLanguageType()
    {
        if(_dropdownIndex == 0)
        {
            languageType = UIManager.LanguageType.English;
        }
        else if(_dropdownIndex == 1)
        {
            languageType = UIManager.LanguageType.Japanese;
        }

        return languageType;
    }

    // 初期化処理
    void InitializeSetting()
    {
        if(UIManager.Instance.CurrentLanguageType == UIManager.LanguageType.English)
        {
            _dropdownIndex = 0;  // English
        }
        else
        {
            _dropdownIndex = 1; // 日本語
        }

        dropdown.value = _dropdownIndex;
    }

}
