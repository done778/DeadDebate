using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance; //싱글톤

    [System.Serializable]
    public class ParticlePool
    {
        public string name; // 생성할 풀 이름
        public GameObject prefab; // 재생할 이펙트파티클프리팹
        public int count = 10; // 미리 생성할 개수
    }

    [SerializeField] private List<ParticlePool> particlePools = new List<ParticlePool>();

    //생성할 풀이름을 Key값으로, Queue로 파티클
    private Dictionary<string, Queue<GameObject>> particlePoolDic = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        //싱그톤
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //파티클풀
        InitializePools();
    }

    private void InitializePools()
    {
        //파티클풀들 돌면서 Queue생성
        foreach (var pool in particlePools)
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            //Queue갯수 지정가능
            for (int i = 0; i < pool.count; i++)
            {
                GameObject gameObject = Instantiate(pool.prefab, transform);
                gameObject.SetActive(false);
                newQueue.Enqueue(gameObject);
            }
            //딕에 풀이름을 Key값으로 등록
            if (!particlePoolDic.ContainsKey(pool.name))
            {
                particlePoolDic.Add(pool.name, newQueue);
            }
        }
    }

    //파티클재생
    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation = default, float autoReturnTime = -1f)
    {
        if (!particlePoolDic.ContainsKey(effectName))
        {
            // Debug.LogError($"{effectName}풀을 찾을 수 없음");
            return null;
        }

        Queue<GameObject> pool = particlePoolDic[effectName];
        GameObject effect;   
        if (pool.Count > 0)
        {
            //남은 파티클이 있다면 Dequeue
            effect = pool.Dequeue();
        }
        else
        {
            //비어있다면 새 파티클 생성
            effect = CreateNew(effectName);
        }

        effect.transform.position = position;
        effect.transform.rotation = rotation;
        effect.SetActive(true);
        SetParticleSystemScale(effect, 0.01f); //파티클 크기조절
        effect.transform.SetParent(null);

        //자동반환계산
        float duration;
        if (autoReturnTime > 0)
        {
            duration = autoReturnTime;
        }
        else
        {
            duration = GetParticleSystemDuration(effect);
        }

        StartCoroutine(ReturnAfterDelay(effectName, effect, duration));

        return effect;
    }
    
    //파티클생성
    private GameObject CreateNew(string effectName)
    {
        ParticlePool poolData = particlePools.Find(p => p.name == effectName);
        if (poolData == null)
        {
            // Debug.LogError($"{effectName}프리팹을 찾을 수 없음");
            return null;
        }

        GameObject gameObject = Instantiate(poolData.prefab, transform);
        gameObject.SetActive(false);
        return gameObject;
    }

    private IEnumerator ReturnAfterDelay(string effectName, GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnEffect(effectName, effect);
    }

    private void ReturnEffect(string effectName, GameObject effect)
    {
        if (!particlePoolDic.ContainsKey(effectName))
            return;

        effect.SetActive(false);
        effect.transform.SetParent(transform);
        particlePoolDic[effectName].Enqueue(effect);
    }
    
    //파티클 지속시간계산
    private float GetParticleSystemDuration(GameObject effect)
    {
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            return particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
        }

        return 2f;
    }

    //파티클 크기조절
    private void SetParticleSystemScale(GameObject effect, float scaleFactor)
    {
        foreach (var particleScale in effect.GetComponentsInChildren<ParticleSystem>())
        {
            var main = particleScale.main;
            main.startSizeMultiplier *= scaleFactor;
            main.startSpeedMultiplier *= scaleFactor;
        }
    }
}
