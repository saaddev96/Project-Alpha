using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OnExit : StateMachineBehaviour
{
    [SerializeField] private eAnimation animation;
    [SerializeField] private bool locklayer;
    [SerializeField] private float crossfade = 0.2f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerStateMachine.instance.StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(stateInfo.length - crossfade);

            AnimatorBrain<eAnimation> target = animator.GetComponentInParent<AnimatorBrain<eAnimation>>();
            target.LockLayer(false, layerIndex);
            target.AnimatorBrainPlay(animation, layerIndex, locklayer, false, crossfade);
        }
        
    }

    
}
