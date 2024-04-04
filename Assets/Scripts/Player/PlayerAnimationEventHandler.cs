using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{

    Footsteps footsteps;
    private void Awake()
    {
        footsteps = GetComponentInParent<Footsteps>();
    }
    void PlayFootsteps()
    {
        footsteps.PlayFootSteps();
    }
}
