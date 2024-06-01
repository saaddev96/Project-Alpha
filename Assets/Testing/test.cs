using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    [SerializeField] private Transform ob;
    public void AddRotation(float angle)
    {
        ob.Rotate(new Vector3(angle, 0, 0));
    }
}
