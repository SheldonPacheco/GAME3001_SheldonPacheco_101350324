using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip gameMusic;
    public AudioClip deathSound;

    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.volume = 0.3f;
        musicSource.Play();
    }
    public void PlayDeathSound()
    {
        musicSource.PlayOneShot(deathSound);
    }
}