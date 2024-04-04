using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum eAnimation
{
    Idle,
    Walk,
    Sprint,
    Crouch,
    Jump,
    Land
}
public class AnimatorBrain : MonoBehaviour
{

    private readonly static int[] animations = new int[Enum.GetNames(typeof(eAnimation)).Length];

    private Animator animator;
    private eAnimation[] currentAnimation;
    private bool[] StatesLocked;
    protected void AnimatorBrainInit(int layersCount,Animator animator, eAnimation startingAnimation)
    {
        int animationsLength = Enum.GetNames(typeof(eAnimation)).Length;
        for (int i = 0; i < animationsLength; i++)
        {
            animations[i] = Animator.StringToHash(Enum.GetName(typeof(eAnimation), i));

        }
        this.animator = animator;
        StatesLocked = new bool[layersCount];
        currentAnimation = new eAnimation[layersCount];
        for (int i = 0; i < layersCount; i++)
        {
            StatesLocked[i] = false;
            this.currentAnimation[i] = startingAnimation;
        }
    }

    protected int GetAnimationHashFromEnum(eAnimation animation)
    {
        return animations[(int)animation];
    }
    public void LockLayer(bool state , int layer)
    {
        StatesLocked[layer] = state;
    }
    public void AnimatorBrainPlay(eAnimation animation,int layer,bool lockAnimation,bool bypass ,float crossfade = 0.2f)
    {

        if (StatesLocked[layer] && !bypass) return;
        if (currentAnimation[layer] == animation) return;
        StatesLocked[layer] = lockAnimation;
        currentAnimation[layer] = animation;
        animator.CrossFade(GetAnimationHashFromEnum(currentAnimation[layer]), crossfade, layer);
    }
}
