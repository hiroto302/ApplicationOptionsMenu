using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// システムクラス UI
public class UIManager : MonoSingleton<UIManager>
{
    public enum LanguageType    // 選択できる言語の種類
    {
        English,
        Japanese
    }

    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private SettingButton _settingButton;
    [SerializeField] private SettingMenu _settingMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private Camera _dummyCamera;

    public LanguageType CurrentLanguageType = LanguageType.English;  // 現在の言語
    public Events.EventDarkMode OnDarkmodeChange;                    // Darkmode が変更される時に発生する event
    public Events.EventLanguageType OnLanguageTypeChange;            // LanguageType が変更される時に発生する event
    public Events.EventLoadFadeComplete OnStartMenuFadeComplete;     // StartMenuのFadeが完了時発生する event


    private bool _isDarkmode = true;  // UI を Darkmode で表示するか
    public bool IsDarkmode
    {
        get { return _isDarkmode; }
        set
        {
            if(_isDarkmode != value)
            {
                _isDarkmode = value;
                DarkmodeDisplay(_isDarkmode);
                OnDarkmodeChange.Invoke(_isDarkmode);  // UIManger の子オブジェクト以外にもDarkmodeを適用
            }
        }
    }

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
        _startMenu.onStartMenuFadeComplete.AddListener(HandleStartMenuFadeComplete);
        GameManager.Instance.OnFirstSceneLoad.AddListener(HandleFirstSceneLoad);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&  GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME && !_startMenu.OnFadeProcessing)
        {
            _startMenu.StartGame();
        }
    }

    // 最初のシーンがロード完了後に実行する処理
    void HandleFirstSceneLoad()
    {
        _startMenu.gameObject.SetActive(true);
        // SetDummyCameraActive(false);             ダミーカメラの切り替えはStartMenu の Animation Eventで実行する
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        // Start Scene => Main Scene 移行して RUNNING の状態に切り替わった時
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            SettingButtonSetActive(true);
        }
        // PAUSED => PREGAME   (Restartボタンが押された時)
        if(previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
        {
            SettingButtonSetActive(false);
            _startMenu.RestartGame();
        }
        // ポーズメニューの表示・非表示
        PauseMenuSetActive(currentState == GameManager.GameState.PAUSED);
    }

    // StartMenu が Fade処理を完了した時の event 処理
    void HandleStartMenuFadeComplete(bool fadeOut, string sceneName)
    {
        OnStartMenuFadeComplete.Invoke(fadeOut, sceneName);  // GameManager・UIManagerに知らせる(仲介の役割)
    }

    public void StartMenuSetActive(bool show)
    {
        _startMenu.gameObject.SetActive(show);
    }

    public void SettingButtonSetActive(bool show)
    {
        _settingButton.gameObject.SetActive(show);
    }

    public void PauseMenuSetActive(bool show)
    {
        _pauseMenu.gameObject.SetActive(show);
    }

    // ここのメソッドだけSettingMenuに対応した書きかたになっている
    // SettingMenuButtonの方の記述を変えて動作するようにすることを検討すること
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

    // DummyCamera の表示・非表示
    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }


    // Darkmode を実行する処理
    public void DarkmodeDisplay(bool darkmode)
    {
        // SaveButtonが押された時,DarkMode が切り替わってる時のみ処理を実行
        ChangeAllTextColor(darkmode);
        ChangeAllImageColor(darkmode);
    }

    // UIMangerが管理してる Text を全て取得し、色を切り替える
    // 注意 : StartMenu の Text には animation で色を制御してるので変更しても、すぐにanimationの設定値に戻る
    private void ChangeAllTextColor(bool darkmode)
    {
        Color textColor = darkmode ? Color.white : Color.black;     // 表示する text color
        Text[] allText = GetComponentsInChildren<Text>(true);       // 子オジェクト全て(非アクティブ含む)の Text 取得
        foreach(var text in allText)
        {
            text.color = textColor;
        }
    }

    private void ChangeAllImageColor(bool darkmode)
    {
        Color imageColor = darkmode ? new Color32(0, 0, 0, 140) : new Color32(255, 255, 255, 140);
        Image[] allImages = GetComponentsInChildren<Image>(true);
        foreach(var image in allImages)
        {
            if(image.gameObject.CompareTag("UIBackground"))
            {
                image.color = imageColor;
            }
        }
    }


    // 表示する言語を切り替える処理
    void UpdateLanguageType(LanguageType type)
    {
        LanguageType previousLanguageType = CurrentLanguageType;
        CurrentLanguageType = type;

        // 変更する前とLanguageType が異なる時の下記の処理を実行
        if(OnLanguageTypeChange != null && CurrentLanguageType != previousLanguageType)
        {
            OnLanguageTypeChange.Invoke(CurrentLanguageType);   // event 発生
        }
    }

    public void ChangeLanguageType(LanguageType type)
    {
        UpdateLanguageType(type);
    }
}
