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
        PlayerStateMachine.OnPlayerLandedEvent += HandleData;
        Footsteps.OnPlayerStepEvent += HandleData;
        Beretta.currentItemSoundEvent+= HandleData;
    }
    private void OnDisable()
    {
        PlayerStateMachine.OnPlayerLandedEvent -= HandleData;
        Footsteps.OnPlayerStepEvent -= HandleData;
        Beretta.currentItemSoundEvent -= HandleData;
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
