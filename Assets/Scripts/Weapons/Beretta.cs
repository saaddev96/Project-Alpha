//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using Cinemachine;

///// <summary>
/////  this script needs more refactoring and Custom Editor
///// </summary>
//public class Beretta : Weapon
//{

//    public enum eWeaponStates
//    {
//        DrawState = 0,
//        SafetySwitchState,
//        FireStartState,
//        FireEndState,
//        FireEndEmptyState,
//        CockingStartState,
//        CockingEndState,
//        MagEjectState,
//        MagInjectState,
//        HolsterState

//    }
//    // Serialized Fields 
//    [Header("Functional Setting")]
//    [SerializeField] private bool canFire = true;
//    [SerializeField] private bool isShotGun = false;
//    [SerializeField] private bool isAuto = false;
//    [SerializeField] private bool isBurst = false;

//    [Header("Magazine Setting")]

//    [SerializeField] private int bulletCount;
//    [SerializeField] private int magSize;

//    [Header("References")]
//    [SerializeField] private Transform firingOrigin;
//    [SerializeField] private GameObject bullet;
//    [SerializeField] private GameObject muzzleFlash;
//    [SerializeField] private GameObject bulletShell;
//    [SerializeField] private GunSound[] gunSounds;

//    [Header("Projectile Setting")]

//    [SerializeField] private float maxFiringDistance= 75;
//    [SerializeField] private float bulletSpeed= 250;
//    [SerializeField] private int PelletsCount = 10;
//    [SerializeField] private float aimConeAngle = 4;
//    [SerializeField] private float fireRate = 15; 
//    [SerializeField] private int BurstBulletCount = 3;

//    [Header("Camera Shake Setting")]

//    [SerializeField] private float shakePower = 0.3f;
//    // None Serialized Fields
//    private float firingTimer = 0;
//    private bool isFiring = false;
//    private bool isTriggerOn = false;
//    private bool isReloading = false;
//    private bool fired = false;
//    private int bulletFired = 0;
//    private int bulletsOnMag;
//    private Vector3 targetPoint;
//    private bool isInspecting;
//    private bool IsSlideOut;
//    private float timeToFireAgain => 1 / fireRate;// bullet/s
//    public bool isAds { get; private set; }
//    private bool isAbleToShootAgain => firingTimer<=0;
//    private bool CanReload => bulletsOnMag < magSize && bulletCount > 0 && !isReloading;
//    public static event Action<AudioData> currentItemSoundEvent;
//    private CinemachineImpulseSource impulseSource;
//    private  void OnEnable()
//    {
//        impulseSource = GetComponent<CinemachineImpulseSource>();
//        IsSlideOut = false;
//        AnimatorBrainInit(item_Anim.layerCount, item_Anim, eItemAnimation.Idle);
//        inputReader.InspectEvent += Inspect;
//        inputReader.AdsEvent += Aim;
//        inputReader.FireEvent += Fire;
//        inputReader.ReloadEvent += Reload;
//    }
  
//    private void OnDisable()
//    {
//        inputReader.InspectEvent -= Inspect;
//        inputReader.AdsEvent -= Aim;
//        inputReader.FireEvent -= Fire;
//        inputReader.ReloadEvent -= Reload;
//        ResetShoot();
//        isReloading = false;
//    }
    
//    private void Start()
//    {

//        bulletsOnMag = magSize;

//    }
//    public override void OnActive()
//    {
//        base.OnActive();
       
//    }
//    public override void OnInactive()
//    {
        
//        base.OnInactive();
  
//    }

//    public override void OnInteract()
//    {
//        base.OnInteract();
//    }

//    protected override void OnMouseOver()
//    {

//    }
//    //public override void Inspect()
//    //{
//    //    isInspecting = true;
//    //}
//    private void Update()
//    {

//        if (isInspecting)
//        {
//            if (PlayerStateMachine.instance.PlayerState != PlayerState.Idle || PlayerStateMachine.instance.IsAdsing)
//            {
//                Arm_animatorbrain.AnimatorBrainStopCurrent(this.itemLayer);
//                AnimatorBrainStopCurrent(0);
//                isInspecting = false;
//                return;
//            }
//            if (Fps_anim.GetCurrentAnimatorStateInfo(this.itemLayer).IsName("Inspect") && Fps_anim.GetCurrentAnimatorStateInfo(this.itemLayer).normalizedTime >= 1.0f - 0.2f)
//            {
//                isInspecting = false;
//            }
//            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Inspect, this.itemLayer, true, false);
//        }

//        if ((!isFiring && fired || isAuto && isAbleToShootAgain) && !isReloading && isTriggerOn)
//        {
//            if (isBurst)
//            {

//                IEnumerator Wait(float time)
//                {
//                    for (int i = 0; i < BurstBulletCount; i++)
//                    {
//                        Shoot();
//                        yield return new WaitForSeconds(time);
//                    }
//                }
//                StartCoroutine(Wait(timeToFireAgain));

//            }
//            else
//            {
//                Shoot();
//            }
//        }
//        if (firingTimer > 0)
//        {
//            firingTimer -= Time.deltaTime;
//        }
//    }

//    //public override void Reload()
//    //{
//    //    if (CanReload)
//    //    {
//    //        if (IsSlideOut)
//    //        {
//    //            AnimatorBrainPlay(eItemAnimation.ReloadEmpty, 0, true, true, 0);
//    //            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.ReloadEmpty, this.itemLayer, true, true, 0);
//    //        }
//    //        else
//    //        {
//    //            AnimatorBrainPlay(eItemAnimation.Reload, 0, true, true, 0);
//    //            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Reload, this.itemLayer, true, true, 0);
//    //        }
//    //        isInspecting = false;

//    //    }
//    //    else if(bulletCount==0)
//    //    {
//    //        // player Need Ammo
//    //    }
//    //}
//    void StartReloading()
//    {
//        isReloading = true;
//    }
//    void applyReload()
//    {
//        // applied using animation event
//        int bulletsToLoad = magSize - bulletsOnMag;
//        int bulletsNeeded = bulletsToLoad < bulletCount ? bulletsToLoad : bulletCount;
//        bulletsOnMag += bulletsNeeded;
//        bulletCount -= bulletsNeeded;
//        isReloading = false;
//        IsSlideOut = false;
//        ResetShoot();
//    }

//    //public override void Fire(bool ctx)
//    //{
//    //    isTriggerOn = ctx;
//    //    fired = true;
//    //}
//    //public override void Aim(bool ctx)
//    //{
//    //    PlayerStateMachine.instance.IsAdsing = ctx;
//    //    isAds = ctx;
//    //}

//    void Shoot()
//    {
//        fired = false;
//        if (bulletsOnMag > 0)
//        {
            
//            PlaySound(eWeaponStates.FireStartState);
//            FiringInstantiation();
//            PlayFiringShake();
//            PlayFiringEffect();
//            PlayFiringAnimation();
//            isFiring = true;
//            bulletFired++;
//            bulletsOnMag--;
//            isInspecting = false;
//            firingTimer = timeToFireAgain;
            
//        }
//        else
//        {
//            // TODO : notify player that he needs ammo
//        }

//    }
//    void FiringInstantiation()
//    {
//        // Finalizing the Firing  calculating direcftion using Ray and instantiating bullet
//        float firingCount = isShotGun ? PelletsCount : 1;
//        float spreadAngle = isShotGun && isAds ? aimConeAngle / 2 : isShotGun ? aimConeAngle : aimConeAngle / 10;
//        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit))
//        {
//            targetPoint = hit.point;
//        }
//        else
//        {
//            targetPoint = ray.GetPoint(maxFiringDistance);
//        }

//        Vector3 directionWithoutSpread = targetPoint - firingOrigin.position;

//        for (int i = 0; i < firingCount; i++)
//        {
//            Vector3 directionWithSpread = RandomDirectionWithinCone(directionWithoutSpread, spreadAngle);
//            GameObject currentBullet = Instantiate(bullet, firingOrigin.position, firingOrigin.rotation);
//            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
//            bulletRb.AddForce(directionWithSpread.normalized * bulletSpeed, ForceMode.Impulse);
//        }
//    }
//    void PlayFiringShake()
//    {
//        // Calculate Shake Velocity and Appliying it
//        float finalShakePower = isAuto ? shakePower / 3 : shakePower;
//        Vector3 shakeVelocity = new Vector3(0.1f, Camera.main.transform.position.y, Camera.main.transform.position.z) * 0.2f * finalShakePower;
//        impulseSource.GenerateImpulseWithVelocity(shakeVelocity);
//    }
//    void PlayFiringEffect()
//    {
//        // Playing Effects
//        muzzleFlash.SetActive(true);
//        bulletShell.SetActive(true);
//    }
//    void PlayFiringAnimation()
//    {
//        // This Function Responsible of Playing Sync Firing Animation of Both Hands And Weapon
//        Arm_animatorbrain.AnimatorBrainStopCurrent(this.itemLayer);
//        AnimatorBrainStopCurrent(0);
//        if (PlayerStateMachine.instance.IsAdsing)
//        {
//            if (bulletsOnMag == 1)
//            {
//                AnimatorBrainPlay(eItemAnimation.FireAdsEmpty, 0, true, false, 0);
//                IsSlideOut = true;
//            }
//            else
//            {
//                AnimatorBrainPlay(eItemAnimation.FireAds, 0, true, false, 0);
//            }
//            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.FireAds, itemLayer, true, false, 0);

//        }
//        else
//        {
//            if (bulletsOnMag == 1)
//            {
//                AnimatorBrainPlay(eItemAnimation.FireEmpty, 0, true, false, 0);
//                IsSlideOut = true;
//            }
//            else
//            {
//                AnimatorBrainPlay(eItemAnimation.Fire, 0, true, false, 0);
//            }
//            Arm_animatorbrain.AnimatorBrainPlay(eAnimation.Fire, itemLayer, true, false, 0);
//        }
//    }
//    Vector3 RandomDirectionWithinCone(Vector3 forward, float angle)
//    {
//        float angleInRadians = angle * Mathf.Deg2Rad; // Convert Angle to Radiant
//        float u = UnityEngine.Random.value; // Get Random Value 0 to 1
//        float v = UnityEngine.Random.value; // Get Random Value 0 to 1
//        float theta = 2 * Mathf.PI * u; // Get Random Theta using u
//        float phi = Mathf.Acos(1 - v * (1 - Mathf.Cos(angleInRadians))); // Getting Random Phi Within Angle 
//        // Spherical Cords Conversion
//        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
//        float y = Mathf.Sin(phi) * Mathf.Sin(theta);
//        float z = Mathf.Cos(phi);

//        Quaternion rotation = Quaternion.LookRotation(forward);

//        Vector3 direction = rotation * new Vector3(x, y, z);

//        return direction.normalized;
//    }
//    void ResetShoot()
//    {
//        isFiring = false;
//        muzzleFlash.SetActive(false);
//        bulletShell.SetActive(false);
//        fired = false;
//    }
//    [System.Serializable]
//    public class GunSound
//    {
//        public eWeaponStates weaponState;
//        public AudioData audioData;
//    }

//    AudioData GetAudioData(eWeaponStates weaponState)
//    {
//        GunSound a = Array.Find(gunSounds, (x) => x.weaponState == weaponState);
//        if (a != null)
//        {
//            return a.audioData;
//        }
//        else
//        {
//            return null;
//        }
//    }

//    // Called By Animation Event
//    void PlaySound(eWeaponStates weaponState)
//    {
//        currentItemSoundEvent?.Invoke(GetAudioData(weaponState));
//    }
//}
