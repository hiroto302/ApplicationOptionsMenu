using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// システムクラス UI
public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private SettingButton _settingButton;
    [SerializeField] private SettingMenu _settingMenu;

    public Events.EventLoadFadeComplete OnStartMenuFadeComplete;   // StartMenuのFadeが完了時発生する event

    public bool Darkmode;  // UI を Darkmode で表示するか

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
        _startMenu.onStartMenuFadeComplete.AddListener(HandleStartMenuFadeComplete);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&  GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            _startMenu.StartGame();
            // GameManager.Instance.StartGame();
        }
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        // Start Scene => Main Scene 移行して RUNNING の状態に切り替わった時
        // if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        // {
        //     _startMenu.gameObject.SetActive(false);
        // }
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            SettingButtonSetActive(true);
        }

        // GameManager.GameState.PREGAMEの時のみ StartMenu を表示
        // _startMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
    }

    // StartMenu が Fade処理を完了した時の event 処理
    void HandleStartMenuFadeComplete(bool fadeOut, string sceneName)
    {
        OnStartMenuFadeComplete.Invoke(fadeOut, sceneName);  // GameManagerに知らせる

        if(fadeOut)
        {
            StartMenuSetActive(!fadeOut);   // StartMenuの表示 非表示
        }
    }

    public void StartMenuSetActive(bool show)
    {
        _startMenu.gameObject.SetActive(show);
    }

    public void SettingButtonSetActive(bool show)
    {
        _settingButton.gameObject.SetActive(show);
    }

    public void SettingMenuSetActive(bool show)
    {
        if(!_settingMenu.gameObject.activeSelf)
        {
            _settingMenu.gameObject.SetActive(show);
        }
        else
        {
            _settingMenu.gameObject.SetActive(!show);
        }
    }


    // Darkmode を実行する処理
    public void DarkmodeDisplay(bool darkmode)
    {
        Debug.Log("Darkmode発令!");
        ChangeAllTextColor(darkmode);

    }
    // UIMangerが管理してる Text を全て取得し、色を切り替える
    // 注意 : StartMenu の Text には animation で色を制御してるので変更しても、すぐにanimationの設定値に戻る
    private void ChangeAllTextColor(bool darkmode)
    {
        Color textColor = darkmode ? Color.white : Color.black;     // 表示する text color
        Text[] allText = GetComponentsInChildren<Text>(true);           // 子オジェクト全て(非アクティブ含む)の Text 取得
        foreach(var text in allText)
        {
            text.color = textColor;
        }
    }
}
