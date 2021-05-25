using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text volumeText;

    public float BGMVolume;     // 設定されてる値

    void Start()
    {
        InitializeSetting();
        // onValueChanged : Action<float>
        slider.onValueChanged.AddListener(HandleBGMSlider);
        SettingMenu.InitializeSettings += InitializeSetting; // SettingMenuが初期化する時に行う、初期化処理を追加
    }
    void HandleBGMSlider(float value)
    {
        // BGMの音量を変更するSoundManagerのメソッドを利用し、音を管理しているシステムクラスから event を発生させる
        // SoundManager.Instance.ChangeBGMVolume(value);
        // 上記の記述をSetting Menu で行う、そのためここで変更されてる内容を記録するのみ
        BGMVolume = value;
        volumeText.text = PercentageValue(BGMVolume).ToString();
    }

    // 値のパーセント化
    int PercentageValue(float value)
    {
        int percentageValue =  Mathf.FloorToInt(value * 100.0f);
        return percentageValue;
    }

    // 初期化処理
    void InitializeSetting()
    {
        BGMVolume = SoundManager.Instance.SettingBGMVolume;
        slider.value = BGMVolume;
        volumeText.text = PercentageValue(BGMVolume).ToString();
    }
}
