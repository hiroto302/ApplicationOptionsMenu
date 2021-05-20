using UnityEngine;

// ScriptableObject : 言語に対応した文字のデータ
[CreateAssetMenu(fileName = "New TextData", menuName = "TextData")]
public class TextData : ScriptableObject
{
    public string EnglishText;  // 英語のテキスト
    public string JapaneseText; // 日本語のテキスト
}
