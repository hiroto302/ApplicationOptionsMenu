using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public Events.EventLoadFadeComplete onStartMenuFadeComplete;    // Fade が完了した時の Event

    private string loadSceneName;  // ロード先にシーン名
    // private string currentSceneName = "Start"; //現在のシーン名

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
        // PAUSED => PREGAME (Restart ボタンが押された時の処理)
        if(previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
        {
            // RestartGame();
            // Debug.Log("StartMenu RestartGame");
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
    }

    // FadeAnimation の完了時の event で呼ぶ method
    public void OnFadeOutComplete()
    {
        onStartMenuFadeComplete.Invoke(true, loadSceneName);
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
}
