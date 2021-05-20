using UnityEngine;
using UnityEngine.UI;

// ScriptableObject TextData に格納されてる文字列を表示させるクラス
public class TextDisplay : MonoBehaviour
{
    [SerializeField] TextData textData;

    [SerializeField] Text displayText;     // 対応した文字を表示させるText

    void OnEnable()
    {
        Initialization();   // 初期化
    }
    void Start()
    {
        // event の追加
        UIManager.Instance.OnLanguageTypeChange.AddListener(HandleLanguageTypeChange);
    }

    // 言語の設定が変更された時に行う処理
    void HandleLanguageTypeChange(UIManager.LanguageType languageType)
    {
        ChangeDisplayText(languageType);
    }

    // 表示する文字の変更
    void ChangeDisplayText(UIManager.LanguageType languageType)
    {
        if(languageType == UIManager.LanguageType.English)
        {
            displayText.text = textData.EnglishText;
        }
        else if(languageType == UIManager.LanguageType.Japanese)
        {
            displayText.text = textData.JapaneseText;
        }
        else
        {
            Debug.LogError("There is no text for the selected language.");
        }
    }

    // 初期化処理
    void Initialization()
    {
        ChangeDisplayText(UIManager.Instance.CurrentLanguageType);
    }
}
