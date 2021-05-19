using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;

    public float BGMVolume;     // 設定されてる値

    void Start()
    {
        // onValueChanged : Action<float>
        slider.onValueChanged.AddListener(HandleBGMSlider);
    }
    void HandleBGMSlider(float value)
    {
        // BGMの音量を変更するSoundManagerのメソッドを利用し、音を管理しているシステムクラスから event を発生させる
        // SoundManager.Instance.ChangeBGMVolume(value);

        // 上記の記述をSetting Menu で行う、そのためここで変更されてる内容を記録するのみ
        BGMVolume = value;
    }
}
