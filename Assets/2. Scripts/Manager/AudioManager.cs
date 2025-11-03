using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource;

    public AudioClip hitSoundEffect;
    public AudioClip dieSoundEffect;

    // 몬스터 동시에 여럿 죽을 때 사운드가 겹쳐져서 불쾌함. 이를 방지하기 위한 최소 간격 변수.
    private float minInterval = 0.05f;
    private float lastPlayTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        lastPlayTime = Time.time;
    }

    public void PlayHitSound()
    {
        sfxSource.PlayOneShot(hitSoundEffect);
    }
    public void PlayDieSound()
    {
        if (Time.time - lastPlayTime > minInterval)
        {
            sfxSource.PlayOneShot(dieSoundEffect);
            lastPlayTime = Time.time;
        }
    }
}
