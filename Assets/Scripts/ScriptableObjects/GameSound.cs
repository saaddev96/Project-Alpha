using System;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSounds", menuName = "Sounds/GameSounds")]
public class GameSound : ScriptableObject
{
    public Sound[] sounds;
    public AudioClip GetSoundClip(string soundname)
    {
        Sound sound = Array.Find(sounds, s => s.soundName == soundname);
        return sound.clip;
    }
    [System.Serializable]
    public struct Sound
    {
        public string soundName;
        public AudioClip clip;
    } 
}
