using UnityEngine;
using System.Collections;

public enum eItemAnimation
{
    Draw,
    Holster,
    Fire,
    FireAds,
    FireEmpty,
    FireAdsEmpty,
    Idle,
    IdleAds,
    Reload,
    ReloadEmpty,
    IdleEmpty
}
public abstract class Item : AnimatorBrain<eItemAnimation>
{
    [SerializeField] protected int itemLayer;
    protected Animator Fps_anim => PlayerStateMachine.instance.FPS_Animator;
    protected Animator item_Anim;
    protected AnimatorBrain<eAnimation> Arm_animatorbrain => (AnimatorBrain<eAnimation>)PlayerStateMachine.instance;
    protected InputReader inputReader => PlayerStateMachine.instance._InputReader;
    protected bool HasAnimator => item_Anim != null;
    protected abstract void OnMouseOver();
    public abstract void OnInteract();
    public abstract void OnActive();
    public abstract void OnInactive();

    public virtual void Awake()
    {
        item_Anim = GetComponent<Animator>();
    }
    public virtual IEnumerator DrawItem()
    {

        PlayerStateMachine.instance.Layer = itemLayer;
        Fps_anim.SetLayerWeight(itemLayer, 1);
        Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Draw, itemLayer, true, true, 0.08f);
        AnimatorBrainPlay(eItemAnimation.Draw, 0, true, true, 0.08f);
        yield return null;
    }
    public virtual IEnumerator HolsterItem()
    {
        Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Holster, itemLayer, true, true, 0.08f);
        AnimatorBrainPlay(eItemAnimation.Holster, 0, true, true, 0.08f);
        yield return new WaitUntil(() => Fps_anim.GetCurrentAnimatorStateInfo(itemLayer).normalizedTime >= 1.0f && Fps_anim.GetCurrentAnimatorStateInfo(itemLayer).IsName("Holster"));
        Fps_anim.SetLayerWeight(itemLayer, 0);
    }
}
