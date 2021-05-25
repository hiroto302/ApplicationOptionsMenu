using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public Events.EventLoadFadeComplete onStartMenuFadeComplete;    // Fade が完了した時の Event

    private string loadSceneName;              // ロード先のシーン名

    public bool OnFadeProcessing = true;       // Fade処理している最中であるか

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {

        // Start Scene => Main Scene 移行して RUNNING の状態に切り替わった時
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
            SoundManager.Instance.FadeInBGMVolume(SoundManager.Instance.InitialBGMVolume, 3.0f);
            SoundManager.Instance.FadeInSEVolume(3.0f);
        }
    }

    public void StartGame()
    {
        this.loadSceneName = "Main";
        FadeIn();
        SoundManager.Instance.FadeOutBGMVolume(SoundManager.Instance.SettingBGMVolume, 3.0f);
        SoundManager.Instance.FadeOutSEVolume(3.0f);
    }

    public void RestartGame()
    {
        this.loadSceneName = "Start";
        FadeIn();
        SoundManager.Instance.FadeOutBGMVolume(SoundManager.Instance.SettingBGMVolume, 3.0f);
        SoundManager.Instance.FadeOutSEVolume(3.0f);
    }

    // Fade処理の実行
    public void FadeOut()
    {
        _animator.SetBool("Fade", true);
        UIManager.Instance.SetDummyCameraActive(false);
    }
    public void FadeIn()
    {
        _animator.SetBool("Fade", false);
        OnFadeProcessing = true;
    }

    // FadeAnimation の完了時の event で実行する method
    public void OnFadeOutComplete()
    {
        onStartMenuFadeComplete.Invoke(true, loadSceneName);
        OnFadeProcessing = false;
    }
    public void OnFadeInComplete()
    {
        UIManager.Instance.SetDummyCameraActive(true);
        onStartMenuFadeComplete.Invoke(false, loadSceneName);

        // Restart 時のFadeIn が完了後の処理
        if(loadSceneName == "Start")
        {
            RestartStartMenu();
        }
    }

    // Restart され再び StartScene に戻ってきた時に行う処理
    void RestartStartMenu()
    {
        _animator.SetBool("Fade", true);
        SoundManager.Instance.FadeInBGMVolume(SoundManager.Instance.InitialBGMVolume, 3.0f);
        SoundManager.Instance.FadeInSEVolume(3.0f);
        SkyBoxManager.Instance.ChangeEveningSky();
        UIManager.Instance.SetDummyCameraActive(false);
        SpawnManager.Instance.RetrunGeneratedPrefab();
    }

    // 初めて StartScen が読み込まれる時の、FadOutAnimation が始まる時時に行う処理
    void OnFadeOutStartFirstStartScene()
    {
        UIManager.Instance.SetDummyCameraActive(false);
    }
    // 初めて StartScen が読み込まれる時の、FadOutAnimation が完了した時に行う処理
    void OnFadeOutCompleteFirstStartScene()
    {
        OnFadeProcessing = false;
    }
}
