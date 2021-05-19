using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioSource aud;

    // public Events.EventBGMValue OnBGMValueChange;


    void Start()
    {
        // onValueChanged : Action<float>
        slider.onValueChanged.AddListener(HandleBGMSlider);
    }
    void HandleBGMSlider(float value)
    {
        // aud = GameObject.Find("EnvironmentSound").GetComponent<AudioSource>();
        // aud.volume = value;

        // 下記の記述は SoundManagerクラスに記述するべきかも
        // 普通に SoundManagerクラスのBGMの音量を変更するメソッドを作って変更するべきかも
        // OnBGMValueChange.Invoke(value);     // Sliderの値（BGMの音量）が変更された時 event 発生

        // BGMの音量を変更するSoundManagerのメソッドを利用し、音を管理しているシステムクラスから event を発生させる
        SoundManager.Instance.ChangeBGMVolume(value);
    }
}
