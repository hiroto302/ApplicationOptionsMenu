using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 最初にロードされる Boot Scene にてインスタンス化し、ゲーム全体を管理するために存在させ続ける
public class GameManager : MonoSingleton<GameManager>
{
    public GameObject[] SystemPrefabs;                 // 生成する他のシステムクラス
    private List<GameObject> _instancedSystemPrefabs;  // 生成したシステムクラスを格納
    private string _currentSceneName;    // 現在のシーン名
    private List<AsyncOperation> _loadOperations;      // ロード時に行う AsyncOperation を格納


    void Start()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        DontDestroyOnLoad(this.gameObject); // GameManagerの常駐化
        InstantiateSystemPrefabs();

        LoadScene("Start");
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

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if(_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
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

        _currentSceneName = seceneName;
    }
}
