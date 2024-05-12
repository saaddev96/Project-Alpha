using UnityEngine;
using System;

public class AnimatorBrain<EAnim> : MonoBehaviour where EAnim : Enum
{

    private readonly static int[] animations = new int[Enum.GetNames(typeof(EAnim)).Length];

    private Animator animator;
    private EAnim[] currentAnimation;
    private bool[] StatesLocked;
    protected void AnimatorBrainInit(int layersCount,Animator animator, EAnim startingAnimation)
    {
        int animationsLength = Enum.GetNames(typeof(EAnim)).Length;
        for (int i = 0; i < animationsLength; i++)
        {
            animations[i] = Animator.StringToHash(Enum.GetName(typeof(EAnim), i));

        }
        this.animator = animator;
        StatesLocked = new bool[layersCount];
        currentAnimation = new EAnim[layersCount];
        for (int i = 0; i < layersCount; i++)
        {
            StatesLocked[i] = false;
            this.currentAnimation[i] = startingAnimation;
        }
    }

    protected int GetAnimationHashFromEnum(EAnim animation)
    {
        return animations[(int)Enum.Parse(typeof(EAnim), Enum.GetName(typeof(EAnim), animation))];
    }
    public void LockLayer(bool state , int layer)
    {
        StatesLocked[layer] = state;
    }
    public void AnimatorBrainPlay(EAnim animation, int layer ,bool lockAnimation,bool bypass ,float crossfade = 0.2f)
    {
        if(animator == null || !animator.HasState(layer, GetAnimationHashFromEnum(currentAnimation[layer])))
        {
            //TODO
            return;
        }
        if (StatesLocked[layer] && !bypass) return;
        if (Enum.GetName(typeof(EAnim), currentAnimation[layer])  == Enum.GetName(typeof(EAnim), animation)) return;
        StatesLocked[layer] = lockAnimation;
        currentAnimation[layer] = animation;
        animator.CrossFade(GetAnimationHashFromEnum(currentAnimation[layer]), crossfade, layer);
        
        
    }

    public void AnimatorBrainStopCurrent(int layer)
    {
        if (currentAnimation[layer] != null)
        {
            animator.StopPlayback();
            LockLayer(false, layer);
        }
    }
}
