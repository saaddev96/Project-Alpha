using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public enum Type
{
    Arm,
    Item
}
public class OnExit : StateMachineBehaviour
{
    [Header("General")]
    [SerializeField] private Type type;
    [Header("Arm General")]
    [SerializeField] private eAnimation Arm_animation;
    [SerializeField] private bool locklayer;
    [SerializeField] private float crossfade = 0.2f;
    [Header("Item General")]
    [SerializeField] private eItemAnimation Item_animation;
    Coroutine waitRoutine;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(type == Type.Arm)
        {

        waitRoutine =PlayerStateMachine.instance.StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);
            AnimatorBrain<eAnimation> target = animator.GetComponentInParent<AnimatorBrain<eAnimation>>();
            target.LockLayer(false, layerIndex);
            target.AnimatorBrainPlay(Arm_animation, PlayerStateMachine.instance.Layer, locklayer, false, crossfade);
            
        }
        }
        if(type == Type.Item)
        {
            waitRoutine = PlayerStateMachine.instance.StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(stateInfo.length - crossfade);
                AnimatorBrain<eItemAnimation> target = animator.GetComponent<AnimatorBrain<eItemAnimation>>();
                target.LockLayer(false, layerIndex);
                target.AnimatorBrainPlay(Item_animation,0, locklayer, false, crossfade);

            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerStateMachine.instance.StopCoroutine(waitRoutine);
        base.OnStateExit(animator, stateInfo, layerIndex);
       
    }
}
