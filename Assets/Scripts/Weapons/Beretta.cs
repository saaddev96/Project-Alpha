using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
public class Beretta : Weapon
{

    public enum eWeaponStates
    {
        DrawState = 0,
        SafetySwitchState,
        FireStartState,
        FireEndState,
        FireEndEmptyState,
        CockingStartState,
        CockingEndState,
        MagEjectState,
        MagInjectState,
        HolsterState

    }
    // Serialized Fields 
    [SerializeField] private bool canFire;
    [SerializeField] private int bulletCount;
    [SerializeField] private int magSize;
    [SerializeField] private float maxFiringDistance;
    [SerializeField] private AnimationCurve sprearCurve;
    [SerializeField] private Transform firingOrigin;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private GameObject bulletShell;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GunSound[] gunSounds;
    // None Serialized Fields
    private bool isFiring = false;
    private bool isReloading = false;
    private int bulletFired = 0;
    private int bulletsOnMag;
    private Vector3 targetPoint;
    private bool isInspecting;
    float curveTimeIteration = 0;
    private bool CanReload => bulletsOnMag < magSize && bulletCount > 0 && !isReloading;
    private bool IsSlideOut;
    public static event Action<AudioData> currentItemSoundEvent;
    private CinemachineImpulseSource impulseSource;
    [SerializeField]private float shakePower;
    private  void OnEnable()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        IsSlideOut = false;
        AnimatorBrainInit(item_Anim.layerCount, item_Anim, eItemAnimation.Idle);
        inputReader.InspectEvent += Inspect;
        inputReader.AdsEvent += Aim;
        inputReader.FireEvent += Fire;
        inputReader.ReloadEvent += Reload;
    }
  
    private void OnDisable()
    {
        inputReader.InspectEvent -= Inspect;
        inputReader.AdsEvent -= Aim;
        inputReader.FireEvent -= Fire;
        inputReader.ReloadEvent -= Reload;
        ResetShoot();
        isReloading = false;
    }
    
    private void Start()
    {

        bulletsOnMag = magSize;

    }
    public override void OnActive()
    {
        base.OnActive();
       
    }
    public override void OnInactive()
    {
        
        base.OnInactive();
  
    }

    public override void OnInteract()
    {
        base.OnInteract();
    }

    protected override void OnMouseOver()
    {

    }
    public override void Inspect()
    {
        isInspecting = true;
    }
    private void Update()
    {

        if (isInspecting)
        {
            if (PlayerStateMachine.instance.PlayerState != PlayerState.Idle || PlayerStateMachine.instance.IsAdsing)
            {
                Arm_animatorbrain.AnimatorBrainStopCurrent(this.itemLayer);
                AnimatorBrainStopCurrent(0);
                isInspecting = false;
                return;
            }
            if (Fps_anim.GetCurrentAnimatorStateInfo(this.itemLayer).IsName("Inspect") && Fps_anim.GetCurrentAnimatorStateInfo(this.itemLayer).normalizedTime >= 1.0f - 0.2f)
            {
                isInspecting = false;
            }
            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Inspect, this.itemLayer, true, false);
        }
    }
    public override void Reload()
    {
        if (CanReload)
        {
            if (IsSlideOut)
            {
                AnimatorBrainPlay(eItemAnimation.ReloadEmpty, 0, true, true, 0);
                Arm_animatorbrain.AnimatorBrainPlay(eAnimation.ReloadEmpty, this.itemLayer, true, true, 0);
            }
            else
            {
                AnimatorBrainPlay(eItemAnimation.Reload, 0, true, true, 0);
                Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Reload, this.itemLayer, true, true, 0);
            }
            isInspecting = false;

        }
        else if(bulletCount==0)
        {
            // player Need Ammo
        }
    }
    void StartReloading()
    {
        isReloading = true;
    }
    void applyReload()
    {
        // applied using animation event
        int bulletsToLoad = magSize - bulletsOnMag;
        int bulletsNeeded = bulletsToLoad < bulletCount ? bulletsToLoad : bulletCount;
        bulletsOnMag += bulletsNeeded;
        bulletCount -= bulletsNeeded;
        isReloading = false;
        IsSlideOut = false;
        ResetShoot();
    }

    public override void Fire()
    {
        Shoot();
    }
    public override void Aim(bool ctx)
    {
        PlayerStateMachine.instance.IsAdsing = ctx;
    }

    void Shoot()
    {
        if (!isFiring && !isReloading)
        {
            if (bulletsOnMag > 0)
            {
                Arm_animatorbrain.AnimatorBrainStopCurrent(this.itemLayer);
                AnimatorBrainStopCurrent(0);
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(maxFiringDistance);
                }

                Vector3 directionWithoutSpread = targetPoint - firingOrigin.position;
                if (curveTimeIteration >= 1)
                {
                    curveTimeIteration = 0;
                }
                Vector3 directionWithSpread = directionWithoutSpread + new Vector3(sprearCurve.Evaluate(curveTimeIteration), sprearCurve.Evaluate(curveTimeIteration + Time.deltaTime), 0);
                curveTimeIteration += Time.deltaTime * 3;
                GameObject currentBullet = Instantiate(Bullet, firingOrigin.position, firingOrigin.rotation);
                Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(directionWithSpread.normalized * bulletSpeed, ForceMode.Impulse);
                Vector3 shakeVelocity = new Vector3(0.1f , Camera.main.transform.position.y,Camera.main.transform.position.z) * sprearCurve.Evaluate(curveTimeIteration)*shakePower;
                impulseSource.GenerateImpulseWithVelocity(shakeVelocity);
                MuzzleFlash.SetActive(true);
                bulletShell.SetActive(true);
                if (PlayerStateMachine.instance.IsAdsing)
                {
                    if (bulletsOnMag == 1)
                    {
                        AnimatorBrainPlay(eItemAnimation.FireAdsEmpty, 0, true, false, 0);
                        IsSlideOut = true;
                    }
                    else
                    {
                        AnimatorBrainPlay(eItemAnimation.FireAds, 0, true, false, 0);
                    }
                    Arm_animatorbrain.AnimatorBrainPlay(eAnimation.FireAds, itemLayer, true, false, 0);

                }
                else
                {
                    if (bulletsOnMag == 1)
                    {
                        AnimatorBrainPlay(eItemAnimation.FireEmpty, 0, true, false, 0);
                        IsSlideOut = true;
                    }
                    else
                    {
                        AnimatorBrainPlay(eItemAnimation.Fire, 0, true, false, 0);
                    }
                    Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Fire, itemLayer, true, false, 0);
                }

                isFiring = true;
                bulletFired++;
                bulletsOnMag--;
                isInspecting = false;
            }
            else
            {

            }
        }
    }
    void ResetShoot()
    {
        isFiring = false;
        MuzzleFlash.SetActive(false);
        bulletShell.SetActive(false);
    }
    [System.Serializable]
    public class GunSound
    {
        public eWeaponStates weaponState;
        public AudioData audioData;
    }

    AudioData GetAudioData(eWeaponStates weaponState)
    {
        GunSound a = Array.Find(gunSounds, (x) => x.weaponState == weaponState);
        if (a != null)
        {
            return a.audioData;
        }
        else
        {
            return null;
        }
    }
    void PlaySound(eWeaponStates weaponState)
    {
        currentItemSoundEvent?.Invoke(GetAudioData(weaponState));
    }
}
