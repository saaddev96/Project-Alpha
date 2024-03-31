using System.Collections.Generic;
using System;
using UnityEngine;

public  abstract class GroundMaterial : MonoBehaviour  
{
    public enum GroundMaterials
    {
        Mud,
        Water,
        Metal,
        Wood,
        Concrete,
        Grass,
        Snow
    }

    public GroundMaterials MaterialType;
    public FootstepsSounds footStepsSound;

}
