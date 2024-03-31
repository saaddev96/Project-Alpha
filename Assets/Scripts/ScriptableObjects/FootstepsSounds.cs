using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FootStepsSounds",menuName ="Sounds/FootSteps")]
public class FootstepsSounds : ScriptableObject
{
    public List<AudioClip> FootSteps = new List<AudioClip>();
}
