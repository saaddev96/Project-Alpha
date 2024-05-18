using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioData : Data
{
    public AudioData(AudioSource source)
    {
        aSource = source;
    }
    public AudioClip clip;
    [HideInInspector]public AudioSource aSource;
    public float volume;
}
