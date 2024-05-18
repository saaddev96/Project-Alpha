using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{


    [Header("Recoil Pattern")]
    [SerializeField]private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilSnapinessSpeed;
    [SerializeField] private float recoilRecoverSpeed;
    [SerializeField] private float hipFireRecoilMultiplier;
    private Vector3 targetRotation;
    private Vector3 currentRotation;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoilRecoverSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSnapinessSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void FireRecoil()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}

