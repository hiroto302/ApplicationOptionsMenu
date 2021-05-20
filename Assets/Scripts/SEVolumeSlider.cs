using UnityEngine;
using UnityEngine.UI;

public class SEVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text volumeText;

    public float SEVolume;     // 設定されてる値

    void Start()
    {
        InitializeSetting();
        // onValueChanged : Action<float>
        slider.onValueChanged.AddListener(HandleBGMSlider);
        SettingMenu.InitializeSettings += InitializeSetting; // SettingMenuが初期化する時に行う、初期化処理を追加
    }
    void HandleBGMSlider(float value)
    {
        // 変更されてる内容を記録
        SEVolume = value;
        volumeText.text = value.ToString();
    }

    // 初期化処理
    void InitializeSetting()
    {
        SEVolume = SoundManager.Instance.SettingSEVolume;
        slider.value = SEVolume;
        volumeText.text = SEVolume.ToString();
    }
}
