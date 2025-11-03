using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource;

    public AudioClip hitSoundEffect;

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
    }

    public void PlayHitSound()
    {
        sfxSource.PlayOneShot(hitSoundEffect);
    }
}
