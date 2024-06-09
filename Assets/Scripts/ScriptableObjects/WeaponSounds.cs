using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New WeaponSounds", menuName = "Sounds/WeaponSounds")]
public class WeaponSounds : ScriptableObject
{
    public Sound[] sounds;
    public AudioData GetSoundClip(eWeaponStates state, AudioSource source)
    {
        Sound sound = Array.Find(sounds, s => s.state == state);
        sound.data.aSource = source;
        return sound.data;
    }
    [System.Serializable]
    public struct Sound
    {
        public eWeaponStates state;
        public AudioData data;
    }
}
