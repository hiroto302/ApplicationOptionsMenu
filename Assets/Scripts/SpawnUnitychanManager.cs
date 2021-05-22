using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unitychan生成を管理する区rす
public class SpawnUnitychanManager : MonoBehaviour
{
    [SerializeField] GameObject _unityChanPrefab;             // ユニティーちゃん
    [SerializeField] GameObject _unityChanContainer;          // 生成するunityChanの親クラス
    [SerializeField] List<GameObject> _unityChanPool;         // 格納する場所

    void Start()
    {
        GenerateUnitychan();
        // ユニティーちゃんを出現を要請された時発生するイベント
        SpawnManager.Instance.OnUnitychanGenerate.AddListener(HandleUnitychanGenerate);
    }

    // event が発生した時の処理
    void HandleUnitychanGenerate(int amountOfPrefabs)
    {
        // SpawnUnitychan(new Vector3(-0.28f, 0, -0.471f));
        SpawnUnitychan(new Vector3(-0.28f, 0, 0));
    }


    // Unitychan の Instance化
    void GenerateUnitychan()
    {
        SpawnManager.Instance.GeneratePrefab(_unityChanPrefab, 1, _unityChanContainer, _unityChanPool);
    }
    // Instance化済みの Unitychan を 渡す
    GameObject RequestUnitychan()
    {
        return SpawnManager.Instance.RequestPrefab(_unityChanPrefab, _unityChanContainer, _unityChanPool);
    }

    // Unitychan の出現
    public void SpawnUnitychan(Vector3 spawnPosition)
    {
        GameObject unitychan = RequestUnitychan();
        unitychan.transform.position = spawnPosition;
    }

    // Unitychan を戻す
    public void ReturnUnitychan()
    {
        // Unitychanが出現してたら false にする
        foreach(var unitychan in _unityChanPool)
        {
            if(unitychan.activeInHierarchy)
            {
                unitychan.SetActive(false);
            }
        }
    }


}
