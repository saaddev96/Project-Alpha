using System;
using UnityEngine;
[CreateAssetMenu(fileName = "GameSounds", menuName = "Sounds/GameSounds")]
public class GameSound : ScriptableObject
{
    public Sound[] sounds;
    public AudioData GetSoundClip(string soundname,AudioSource source)
    {
        Sound sound = Array.Find(sounds, s => s.soundName == soundname);
        sound.data.aSource = source;
        return sound.data;
    }
    public AudioData GetSoundClip(Enum state, AudioSource source)
    {
        Sound sound = Array.Find(sounds, s => s.soundName == state.ToString());
        sound.data.aSource = source;
        return sound.data;
    }
    [System.Serializable]
    public struct Sound
    {
        public string soundName;
        public AudioData data;
    } 

}
