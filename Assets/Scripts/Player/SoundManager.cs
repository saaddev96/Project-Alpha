using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Game Sound Setting")]
    [SerializeField] private bool muted = false;
    [Space]
    [Header("Game Sounds")]
    [SerializeField] private GameSound PlayerSounds;
    [Space]
    private AudioSource audioSource;
    private float volume;

    private void Start()
    {
        volume = 0.1f;// TODO : volume should be Controlled in setting
    }
    public void PlayerSfxOnce(AudioClip Sfxclip,AudioSource aSource)
    {
        if (!muted)
        {
        audioSource = aSource;
        audioSource.PlayOneShot(Sfxclip, volume);
        }
    }
    public void StopSoundEffect()
    {
        audioSource.Stop();
    }
}
