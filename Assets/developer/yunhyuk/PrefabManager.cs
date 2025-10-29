using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public List<GameObject> prefabs;

    private void Awake()
    {
        GameManager.Instance.RegistPrefabManager(this);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
