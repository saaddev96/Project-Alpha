using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Game Sound Setting")]
    [SerializeField] private bool muted = false;

    private void Start()
    {
    }
    private void OnEnable()
    {
        Movement.OnPlayerLandedEvent += HandleData;
        Footsteps.OnPlayerStepEvent += HandleData;
    }
    private void OnDisable()
    {
        Movement.OnPlayerLandedEvent -= HandleData;
        Footsteps.OnPlayerStepEvent -= HandleData;
    }
 

    void HandleData(Data data)
    {
        if(data is AudioData audioData)
        {
            PlayerSfxOnce(audioData);
        }
    }
    public void PlayerSfxOnce(AudioData data)
    {
        if (!muted)
        {
            data.aSource.PlayOneShot(data.clip, data.volume);

        }
    }
  

}
