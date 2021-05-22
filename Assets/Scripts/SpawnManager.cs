using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シーン上に生成する Object を管理するシステムクラス
public class SpawnManager : MonoSingleton<SpawnManager>
{
    // [SerializeField] GameObject[] _prefabs;

    [SerializeField] SpawnUnitychanManager _spawnUnitychanManager;

    public Events.EventGeneratePrefab OnPrefabGenerate;
    public Events.EventGeneratePrefab OnUnitychanGenerate;

    [SerializeField] GameObject _fairyPrefab;
    [SerializeField] GameObject _fairyContainer;
    [SerializeField] List<GameObject> _fairyPool;


    public void GenerateUnitychan(int amountOfPrefabs)
    {
        OnUnitychanGenerate.Invoke(amountOfPrefabs);
    }


    // Object Pool を実装 するためのメソッド
    // prefab インスタンス化・生成
    public void GeneratePrefab(GameObject prefab, int amountOfPrefabs, GameObject container ,List<GameObject> pool)
    {
        for(int i = 0; i < amountOfPrefabs; i++)
        {
            GameObject generatedPrefab = Instantiate(prefab) as GameObject;
            generatedPrefab.transform.parent = container.transform;
            generatedPrefab.SetActive(false);
            pool.Add(generatedPrefab);
        }
    }

    // prefab を要請された時に渡す
    public GameObject RequestPrefab(GameObject prefabInPool, GameObject container, List<GameObject> pool )
    {
        foreach(var prefab in pool)
        {
            if(prefab.activeInHierarchy == false)
            {
                prefab.SetActive(true);
                return prefab;
            }
        }

        GameObject newPrefab = Instantiate(prefabInPool) as GameObject;
        newPrefab.transform.parent = container.transform;
        pool.Add(newPrefab);
        return newPrefab;
    }
}
