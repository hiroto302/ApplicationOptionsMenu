using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 音を管理するシステムクラス
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] float initialBGMVolume = 0.7f;   // BGM の音量の初期値
    [SerializeField] float initialSEVolume = 0.7f;    // SE の音量の初期値
    public float SettingBGMVolume;                    // 各BGMのクラスに設定する値
    public float SettingSEVolume;                     // 各SEのクラスに設定する値
    public Events.EventBGMValue OnBGMVolumeChange;    // BGM の音量が変更された時の event


    protected override void Awake()
    {
        base.Awake();

        SettingBGMVolume = initialBGMVolume;
        SettingSEVolume = initialSEVolume;
    }


    /*下記にしるすように、毎回音量の値が変わった時、
        このシステムクラスが他の音量を扱う全てのオブジェクトを取得する必要はない
        値が変わったとき、発生する event を作り、
        その event が 発生した時に全て BGM クラスが知らせを受けて各自の音量を変えるように実装した */
    public void ChangeBGMVolume(float volume)
    {
        SettingBGMVolume = volume;
        OnBGMVolumeChange.Invoke(SettingBGMVolume);   // 値が変更された時, BGM の音量を変更する event 発生
    }
}
