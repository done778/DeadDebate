using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    private Dictionary<string, GameObject> dictPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        GameManager.Instance.RegistPrefabManager(this);
        foreach (GameObject prefab in prefabs)
        {
            dictPrefabs.Add(prefab.name, prefab);
        }
    }

    public GameObject GetPrefabFromKey(string name)
    {
        return dictPrefabs.GetValueOrDefault(name);
    }
}
