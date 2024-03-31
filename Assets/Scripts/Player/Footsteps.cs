using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
  
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private AudioSource audioSource;
    private FootstepsSounds footStepSound;
    GroundMaterial Material;
    GroundMaterial currentMaterial;

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundMask))
        {
            if(hit.collider.TryGetComponent<GroundMaterial>(out Material))
            {
                if(currentMaterial == null)
                {
                    soundIteration = 0;
                    currentMaterial = Material;
                    footStepSound = currentMaterial.footStepsSound;
                }
                else if(Material.MaterialType != currentMaterial.MaterialType)
                {
                    soundIteration = 0;
                    currentMaterial = Material;
                    footStepSound = currentMaterial.footStepsSound;
                }
            }
            else
            {
                footStepSound = null;
                currentMaterial = null;
            }
        }
    }
    int soundIteration = 0;
    void PlayFootSteps()
    {
        if (footStepSound != null)
        {
            SoundManager.Instance.PlayerSfxOnce(footStepSound.FootSteps[soundIteration], audioSource);
            if(footStepSound.FootSteps.Count-1 > soundIteration)
            {
                soundIteration++;
            }
            else
            {
                soundIteration = 0;
            }
        }
    }
}
