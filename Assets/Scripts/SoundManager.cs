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
    public Events.EventSEValue OnSEVolumeChange;      // SE の音量が変更された時の event
    private Coroutine fadeBGMVolumeCoroutine;
    private Coroutine fadeSEVolumeCoroutine;

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
        FadeInBGMVolume(initialBGMVolume, 3.0f);      // 音をfadeInさせながら初期シーン開始
        FadeInSEVolume(3.0f);
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
    public void ChangeSEVolume(float volume)
    {
        SettingSEVolume = volume;
        OnSEVolumeChange.Invoke(SettingSEVolume);   // 値が変更された時, SE の音量を変更する event 発生
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
        fadeBGMVolumeCoroutine = StartCoroutine(FadeInBGMVolumeCoroutine(volume, second));
    }
    public void FadeOutSEVolume(float second)
    {
        initialSEVolume = SettingSEVolume;
        fadeSEVolumeCoroutine = StartCoroutine(FadeOutSEVolumeCoroutine(SettingSEVolume, second));
    }
    public void FadeInSEVolume(float second)
    {
        fadeSEVolumeCoroutine = StartCoroutine(FadeInSEVolumeCoroutine(initialSEVolume, second));
    }

    // initialVolume を 0 にする
    IEnumerator FadeOutBGMVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeOutRate = initialVolume / second;     // fade率 (FadeOut: 減少率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(elapsedTime <= second)
        {
            // fadedVolume : フェードした後の音量
            float fadedVolume = initialVolume - fadeOutRate * elapsedTime;
            ChangeBGMVolume(fadedVolume);                    // 減少した音に設定値を変更
            elapsedTime += waitForSecond;                    // 経過時間の増加
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
        }
        ChangeBGMVolume(0);                 // 0 にする
        StopCoroutine(fadeBGMVolumeCoroutine);       // fade 処理終了
        fadeBGMVolumeCoroutine = null;
    }

    // 0 から initialVolume (fadeOutする前の初期値) まで音量を大きくする
    IEnumerator FadeInBGMVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeInRate = initialVolume / second;     // fade率 (FadeIn: 増加率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(elapsedTime <= second)
        {
            // fadedVolume : フェードした後の音量
            float fadedVolume = 0 + fadeInRate * elapsedTime;
            ChangeBGMVolume(fadedVolume);                    // 減少した音に設定値を変更
            elapsedTime += waitForSecond;                    // 経過時間の増加
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
        }
        ChangeBGMVolume(initialVolume);     // 初期値の音量を戻す
        StopCoroutine(fadeBGMVolumeCoroutine);       // fade 処理終了
        fadeBGMVolumeCoroutine = null;
    }
    // initialVolume を 0 にする
    IEnumerator FadeOutSEVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeOutRate = initialVolume / second;     // fade率 (FadeOut: 減少率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(elapsedTime <= second)
        {
            // fadedVolume : フェードした後の音量
            float fadedVolume = initialVolume - fadeOutRate * elapsedTime;
            ChangeSEVolume(fadedVolume);                    // 減少した音に設定値を変更
            elapsedTime += waitForSecond;                    // 経過時間の増加
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
        }
        ChangeSEVolume(0);                 // 0 にする
        StopCoroutine(fadeSEVolumeCoroutine);       // fade 処理終了
        fadeSEVolumeCoroutine = null;
    }

    // 0 から initialVolume (fadeOutする前の初期値) まで音量を大きくする
    IEnumerator FadeInSEVolumeCoroutine(float initialVolume, float second)
    {
        float elapsedTime = 0;                          // 経過時間
        float fadeInRate = initialVolume / second;     // fade率 (FadeIn: 増加率)
        float waitForSecond = 0.1f;                     // 待機時間
        while(elapsedTime <= second)
        {
            // fadedVolume : フェードした後の音量
            float fadedVolume = 0 + fadeInRate * elapsedTime;
            ChangeSEVolume(fadedVolume);                    // 減少した音に設定値を変更
            elapsedTime += waitForSecond;                    // 経過時間の増加
            yield return new WaitForSeconds(waitForSecond);  // 0.1秒毎に処理を繰り返す
        }
        ChangeSEVolume(initialVolume);     // 初期値の音量を戻す
        StopCoroutine(fadeSEVolumeCoroutine);       // fade 処理終了
        fadeSEVolumeCoroutine = null;
    }
    #endregion
}
