using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource soundFXSource;

    public AudioClip gameMusic;
    public AudioClip walkingSound;
    public AudioClip goalReached;
    public AudioClip buttonPress;
    public static SoundManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);     
        soundFXSource.volume = 0.3f;
        musicSource.volume = 0.4f;

    }

    void Start()
    {
        PlayMusic(gameMusic);
    }
    public void PlaySFX(AudioClip audioClip)
    {
        soundFXSource.PlayOneShot(audioClip);  
    }
    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.volume = 0.4f;
        musicSource.PlayOneShot(audioClip);
        musicSource.loop = true;
    }
    public void StopMusic(AudioClip audioClip)
    {
        musicSource.Stop();
    }
    public void StopSFX(AudioClip audioClip)
    {
        soundFXSource.Stop();
    }
}