using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioData : Data
{
    public AudioClip clip;
    public AudioSource aSource;
    public float volume;
}
