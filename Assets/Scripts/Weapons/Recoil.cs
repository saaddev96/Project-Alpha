using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private float RecoilX => CurrentWeaponType.recoilX;
    private float RecoilY => CurrentWeaponType.recoilY;
    private float RecoilZ => CurrentWeaponType.recoilZ;
    private float RecoilSnapinessSpeed => CurrentWeaponType.recoilSnapinessSpeed;
    private float RecoilRecoverSpeed => CurrentWeaponType.recoilRecoverSpeed;
    private float HipFireRecoilMultiplier => CurrentWeaponType.hipFireRecoilMultiplier;
    private Weapon CurrentWeapon
    {
        get {

            Weapon weapon = PlayerSlotManager.Instance.CurrentItem as Weapon;
            if(weapon != null)
            {
                return weapon;
            }
            else
            {
                return null;
            }
        }
    
    }
    private WeaponType CurrentWeaponType => CurrentWeapon.P_WeaponType;

    private Vector3 targetRotation;
    private Vector3 currentRotation;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (CurrentWeapon == null) return;
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, RecoilRecoverSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, RecoilSnapinessSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void FireRecoil()
    {

        Vector3 randRecoil = new Vector3(RecoilX, Random.Range(-RecoilY, RecoilY), Random.Range(-RecoilZ, RecoilZ));
        if (CurrentWeapon.IsAds)
        {
            randRecoil /= HipFireRecoilMultiplier;
        }
        targetRotation += randRecoil;
    }
}

