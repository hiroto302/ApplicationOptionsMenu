using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SEを制御するスクリプト
public class SE : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] AudioClip se = null;

    void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource.clip == null)
        {
            audioSource.clip = se;
        }
        // 各初期値設定
        audioSource.priority = 2;           // 優先度
        audioSource.volume = 0.5f;          // 音量
        audioSource.loop = false;            // ループ化
        audioSource.spatialBlend = 0;       // 音の2D化
        audioSource.playOnAwake = false;     // 再生タイミング
    }

    void OnEnable()
    {
        /* 細かく音量の値を設定したければ、初期値の値からどれほど大きくするか、のような記述を追加するべき */
        // 音量の設定
        audioSource.volume = SoundManager.Instance.SettingSEVolume;

    }
    void Start()
    {
        SoundManager.Instance.OnSEVolumeChange.AddListener(HandleVolumeChange);  // event の追加
    }
    void HandleVolumeChange(float volume)
    {
        audioSource.volume = volume;
        Debug.Log("SE音量が変更されたよ");
    }
}
