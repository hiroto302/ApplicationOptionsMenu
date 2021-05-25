using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// 音を管理するシステムクラス
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] float initialBGMVolume = 0.7f;   // BGM の音量の初期値
    [SerializeField] float initialSEVolume = 0.7f;    // SE の音量の初期値
    public float SettingBGMVolume;                    // 各BGMのクラスに設定する値
    public float SettingSEVolume;                     // 各SEのクラスに設定する値
    public Events.EventBGMValue OnBGMVolumeChange;    // BGM の音量が変更された時の event
    public Events.EventSEValue OnSEVolumeChange;      // SE の音量が変更された時の event
    private Coroutine fadeBGMVolumeCoroutine;
    private Coroutine fadeSEVolumeCoroutine;

    private Action ExecuteWait;

    public float InitialBGMVolume
    {
        get { return  initialBGMVolume; }
        set { initialBGMVolume = value; }
    }
    public float InitialSEVolume
    {
        get { return  initialSEVolume; }
        set { initialSEVolume = value; }
    }


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        SettingBGMVolume = initialBGMVolume;
        SettingSEVolume = initialSEVolume;

        GameManager.Instance.OnFirstSceneLoad.AddListener(HandleFirstSceneLoad);
    }

    // 最初のシーンがロード完了後に行う処理
    void HandleFirstSceneLoad()
    {
        FadeInBGMVolume(initialBGMVolume, 3.0f);      // 音をfadeInさせながら初期シーン開始
        FadeInSEVolume(3.0f);
    }

    /*下記にしるすように、毎回音量の値が変わった時、
        このシステムクラスが他の音量を扱う全てのオブジェクトを取得する必要はない
        値が変わったとき、発生する event を作り、
        その event が 発生した時に全て BGM クラスが知らせを受けて各自の音量を変えるように実装 */
    public void ChangeBGMVolume(float volume)
    {
        SettingBGMVolume = volume;
        OnBGMVolumeChange.Invoke(SettingBGMVolume);   // 値が変更された時, BGM の音量を変更する event 発生
    }
    public void ChangeSEVolume(float volume)
    {
        SettingSEVolume = volume;
        OnSEVolumeChange.Invoke(SettingSEVolume);    // 値が変更された時, SE の音量を変更する event 発生
    }

    // 処理のリファクタリングを検討すること
    #region Fade Sound Volume Methods & Coroutines
    /*  音 Fade Out : 音を徐々に小さくする
        色 Fade Out : Aの値（透明度) を徐々に小さくする ex) 黒 => 透明 */

    // second : fadeを完了するまでの時間、  volume : fade対象の音量
    public void FadeOutBGMVolume(float volume, float second)
    {
        initialBGMVolume = volume;   // Fadeさせる前の初期値を記録
        fadeBGMVolumeCoroutine = StartCoroutine(FadeOutBGMVolumeCoroutine(volume, second));
    }
    // 音 Fade In : 音を徐々に大きくする
    public void FadeInBGMVolume(float volume, float second)
    {
        // fadeBGmVolumeが参照されている場合待機状態, Nullになったら(FadeOut処理が終了したら)FadeIn処理を開始
        if(fadeBGMVolumeCoroutine != null)
        {
            // 処理を実行するまで待機状態
            StartCoroutine(FadeInBGMVolumeExecutionWaitingRoutine(volume, second));
        }
        else
        {
            fadeBGMVolumeCoroutine = StartCoroutine(FadeInBGMVolumeCoroutine(volume, second));
        }
    }

    IEnumerator FadeInBGMVolumeExecutionWaitingRoutine(float volume, float second)
    {
        while(fadeBGMVolumeCoroutine != null)
        {
            // 待機状態
            yield return null;
        }
        // FadeOutの処理が完了した後実行
        fadeBGMVolumeCoroutine = StartCoroutine(FadeInBGMVolumeCoroutine(volume, second));
        // 待機処理の終了
        StopCoroutine(FadeInBGMVolumeExecutionWaitingRoutine(volume, second));
    }


    public void FadeOutSEVolume(float second)
    {
        initialSEVolume = SettingSEVolume;
        fadeSEVolumeCoroutine = StartCoroutine(FadeOutSEVolumeCoroutine(SettingSEVolume, second));
    }

    public void FadeInSEVolume(float second)
    {
        if(fadeSEVolumeCoroutine != null)
        {
            // 処理を実行するまで待機状態
            StartCoroutine(FadeInSEVolumeExecutionWaitingRoutine(initialSEVolume ,second));
        }
        else
        {
            fadeSEVolumeCoroutine = StartCoroutine(FadeInSEVolumeCoroutine(initialSEVolume ,second));
        }
    }

    IEnumerator FadeInSEVolumeExecutionWaitingRoutine(float volume, float second)
    {
        while(fadeSEVolumeCoroutine != null)
        {
            yield return null;
        }
        fadeSEVolumeCoroutine = StartCoroutine(FadeInSEVolumeCoroutine(volume, second));
        StopCoroutine(FadeInSEVolumeExecutionWaitingRoutine(volume, second));
    }

    // initialVolume を 0 にする (initialVolume には 現在各オブジェクトに適用されてる音量 SettingBGMVolume が代入する)
    IEnumerator FadeOutBGMVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeOutRate = initialVolume / second;     // fade率 (FadeOut: 減少率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(true)
        {
            elapsedTime += waitForSecond;                   // 経過時間の増加
            if(elapsedTime >= second)
            {
                break;
            }
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
            // fadedVolume : フェードした後の音量
            float fadedVolume = initialVolume - fadeOutRate * elapsedTime;
            ChangeBGMVolume(fadedVolume);                    // 減少した音に設定値を変更
        }
        ChangeBGMVolume(0);                          // 0 にする
        StopCoroutine(fadeBGMVolumeCoroutine);       // fade 処理終了
        fadeBGMVolumeCoroutine = null;
    }

    // 0 から initialVolume (fadeOutする前の初期値) まで音量を大きくする
    IEnumerator FadeInBGMVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeInRate = initialVolume / second;      // fade率 (FadeIn: 増加率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(true)
        {
            elapsedTime += waitForSecond;                    // 経過時間の増加
            if(elapsedTime >= second)
            {
                break;
            }
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
            // fadedVolume : フェードした後の音量
            float fadedVolume = 0 + fadeInRate * elapsedTime;
            ChangeBGMVolume(fadedVolume);                    // 減少した音に設定値を変更
        }
        ChangeBGMVolume(initialVolume);              // 初期値の音量を戻す
        StopCoroutine(fadeBGMVolumeCoroutine);       // fade 処理終了
        fadeBGMVolumeCoroutine = null;
    }
    // initialVolume を 0 にする
    IEnumerator FadeOutSEVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeOutRate = initialVolume / second;     // fade率 (FadeOut: 減少率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(true)
        {
            elapsedTime += waitForSecond;                    // 経過時間の増加
            if(elapsedTime >= second)
            {
                break;
            }
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
            // fadedVolume : フェードした後の音量
            float fadedVolume = initialVolume - fadeOutRate * elapsedTime;
            ChangeSEVolume(fadedVolume);                    // 減少した音に設定値を変更
        }
        ChangeSEVolume(0);                           // 0 にする
        StopCoroutine(fadeSEVolumeCoroutine);       // fade 処理終了
        fadeSEVolumeCoroutine = null;
    }

    // 0 から initialVolume (fadeOutする前の初期値) まで音量を大きくする
    IEnumerator FadeInSEVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeInRate = initialVolume / second;      // fade率 (FadeIn: 増加率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(true)
        {

            elapsedTime += waitForSecond;                    // 経過時間の増加
            if(elapsedTime >= second)
            {
                break;
            }
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
            // fadedVolume : フェードした後の音量
            float fadedVolume = 0 + fadeInRate * elapsedTime;
            ChangeSEVolume(fadedVolume);                     // 減少した音に設定値を変更
        }
        ChangeSEVolume(initialVolume);              // 初期値の音量を戻す
        StopCoroutine(fadeSEVolumeCoroutine);       // fade 処理終了
        fadeSEVolumeCoroutine = null;
    }
    #endregion
}
