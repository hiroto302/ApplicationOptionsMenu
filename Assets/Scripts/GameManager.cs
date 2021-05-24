using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 最初にロードされる Boot Scene にてインスタンス化し、ゲーム全体を管理するために存在させ続ける
public class GameManager : MonoSingleton<GameManager>
{

    public enum GameState  //Game の状態
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;                      // 生成する他のシステムクラス
    private List<GameObject> _instancedSystemPrefabs;       // 生成したシステムクラスを格納
    private string _currentSceneName;                       // 現在のシーン名
    [SerializeField] private GameState _currentGameState = GameState.PREGAME;    // 現在のゲームの状態
    private List<AsyncOperation> _loadOperations;           // ロード時に行う AsyncOperation を格納
    public Events.EventGamState OnGameStateChange;          // GameState が変わる時発生する event

    public int LoadedManiSceneCount;                        // メインシーンをロードした回数

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    public string CurrentSceneName
    {
        get { return _currentSceneName; }
        private set { _currentSceneName = value; }
    }


    void Start()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();


        DontDestroyOnLoad(this.gameObject); // GameManagerの常駐化
        InstantiateSystemPrefabs();
        LoadScene("Start");

        UIManager.Instance.OnStartMenuFadeComplete.AddListener(HandleStartMenuFadeComplete);  // Fade処理が完了時の処理
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;  // ゲームの状態の移り変わりを把握したいため
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0f;    // Pause simulation when in pause state
                break;
            default:
                break;
        }

        // event
        OnGameStateChange.Invoke(_currentGameState, previousGameState);
    }

    // SystemClassの 生成
    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        foreach (var systemPrefab in SystemPrefabs)
        {
            prefabInstance = Instantiate(systemPrefab);
            // 生成したものを格納
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    // シーンのロード完了時に行う処理
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if(_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if(_loadOperations.Count == 0 && _currentSceneName == "Main")
            {
                LoadedManiSceneCount++;
                UpdateState(GameState.RUNNING);
                // Unitychan を要請
                if(LoadedManiSceneCount % 2 == 0)
                {
                    SpawnManager.Instance.GenerateUnitychan(LoadedManiSceneCount);
                    SpawnManager.Instance.GenerateFairy(LoadedManiSceneCount);
                }
            }
        }
    }

    // シーンのアンロード完了時時に行う処理
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        // Debug.Log("_loadOperations.Count : " +  _loadOperations.Count);
    }

    public void LoadScene(string seceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(seceneName, LoadSceneMode.Additive);
        if ( ao == null)
        {
            Debug.LogError("[GameManager] Unable to load Scene " + seceneName);
            return;
        }
        ao.completed += OnLoadOperationComplete;    // ロード完了時の event 処理
        _loadOperations.Add(ao);

        _currentSceneName = seceneName;  // 現在のシーン名を、次に読み込まれるシーン名に変更
    }

    public void UnLoadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if(ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level" + sceneName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }


    // Fade処理が完了した時に行う処理
    void HandleStartMenuFadeComplete(bool fadeOut, string sceneName)
    {
        // FadeIn の場合
        if(!fadeOut)
        {
            UnLoadScene(_currentSceneName); // 現在読み込んでるシーンを取り除く
            LoadScene(sceneName);           // FadeInが終わった後に次のシーンを読み込む
        }
    }
    public void StartGame()
    {
        LoadScene("Main");
    }

    // ポーズメニューの切り替え
    public void TogglePause()
    {
        // Restartの状態ではPause メニューは開けない
        if(CurrentGameState == GameState.RUNNING || CurrentGameState == GameState.PAUSED)
        {
            UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
        }
    }


    // 最初の画面(ここでは、Start Scene)に戻って再スタート
    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);     // PREGAMEに戻った時の処理を各クラスに実装する
    }

    // implement features for quitting
    public void QuitGame()
    {
        Application.Quit();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach(var systemPrefab in _instancedSystemPrefabs)
        {
            Destroy(systemPrefab);
        }
        _instancedSystemPrefabs.Clear();
    }
}
