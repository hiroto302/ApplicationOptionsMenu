using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFairyManager : MonoBehaviour
{
    [SerializeField] GameObject _fairyPrefab;
    [SerializeField] GameObject _fairyContainer;
    [SerializeField] List<GameObject> _fairyPool;


    void Start()
    {
        GenerateFairy(10);
        // ユニティーちゃんを出現を要請された時発生するイベント
        SpawnManager.Instance.OnFairyGenerate.AddListener(HandleFairyGenerate);
    }

    // event が発生した時の処理
    void HandleFairyGenerate(int amountOfPrefabs)
    {
        for(int i = 0; i < amountOfPrefabs; i++)
        {
            SpawnFairy(SpawnRandomPosition());
        }
    }


    // Instance化
    void GenerateFairy(int amountOfPrefab)
    {
        SpawnManager.Instance.GeneratePrefab(_fairyPrefab, amountOfPrefab, _fairyContainer, _fairyPool);
    }
    // Instance化済みの Fairy を 渡す
    GameObject RequestUnitychan()
    {
        return SpawnManager.Instance.RequestPrefab(_fairyPrefab, _fairyContainer, _fairyPool);
    }

    // Fairy の出現
    public void SpawnFairy(Vector3 spawnPosition)
    {
        GameObject unitychan = RequestUnitychan();
        unitychan.transform.position = spawnPosition;
    }

    // 出現させるランダムな位置
    Vector3 SpawnRandomPosition()
    {
        return new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.5f, 2.5f), Random.Range(2f, 3f));
    }

    // 出現させた Fairy を戻す
    public void ReturnFairy()
    {
        foreach(var fairy in _fairyPool)
        {
            if(fairy.activeInHierarchy)
            {
                fairy.SetActive(false);
            }
        }
    }
}
