using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/New Weapon")]
public class WeaponType : ScriptableObject
{
    [Header("Functional Setting")]

     public bool isShotGun = false;
     public bool isAuto = false;
     public bool isBurst = false;


    [Header("References")]

    public GameObject bullet;
    public GameSound WeaponSounds;

    [Header("Magazine Setting")]

     public int bulletCount;
     public int magSize;

    [Header("Projectile Setting")]

     public float maxFiringDistance = 75;
     public float bulletSpeed = 250;
     public int PelletsCount = 10;
     public float aimConeAngle = 4;
     public float fireRate = 15;
     public int BurstBulletCount = 3;

    [Header("Recoil Pattern")]
    public float recoilX= -2f;
    public float recoilY= 2f;
    public float recoilZ=0.2f;

    [Header("Recoil Settings")]

    public float recoilSnapinessSpeed = 6f;
    public float recoilRecoverSpeed = 1f;
    public float hipFireRecoilMultiplier = 2f;

    [Header("Camera Shake Setting")]

    [Range(0,10)] public float shakePower = 1f;

    [Header("Aim Setting")]

     public int AdsFOV = 45;
}
