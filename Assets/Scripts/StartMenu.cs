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
        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeIn();
        }

        // Start Scene => Main Scene 移行して RUNNING の状態に切り替わった時
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }
    }

    public void StartGame()
    {
        this.loadSceneName = "Main";
        FadeIn();
    }

    // Fade処理の実行
    public void FadeOut()
    {
        _animator.SetBool("Fade", true);
    }
    public void FadeIn()
    {
        _animator.SetBool("Fade", false);
    }

    // FadeAnimation の完了時の event で呼ぶ method
    public void OnFadeOutComplete()
    {
        Debug.Log("FadeOut 完了");
        onStartMenuFadeComplete.Invoke(true, loadSceneName);
    }
    public void OnFadeInComplete()
    {
        Debug.Log("Fade In 完了");
        onStartMenuFadeComplete.Invoke(false, loadSceneName);
    }

}
