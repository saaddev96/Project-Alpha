using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
public  class Weapon : Item
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
    [Header("Functional Setting")]
    [SerializeField] private WeaponType weapon;
    [SerializeField] private bool canFire = true;
    [SerializeField]private bool audioMute = false;
    [SerializeField] private Transform firingOrigin;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject bulletShell;
    private bool IsShotGun => weapon.isShotGun;
    private bool IsAuto => weapon.isAuto;
    private bool IsBurst => weapon.isBurst;
    private int BulletCount => weapon.bulletCount;

    private int MagSize=> weapon.magSize;
    private GameObject Bullet=> weapon.bullet;
    private float MaxFiringDistance => weapon.maxFiringDistance;
    private float BulletSpeed => weapon.bulletSpeed;
    private int PelletsCount => weapon.PelletsCount;
    private float AimConeAngle => weapon.aimConeAngle;
    private float FireRate => weapon.fireRate;
    private int BurstBulletCount => weapon.BurstBulletCount;
    private float ShakePower => weapon.shakePower;
    private int AdsFOV => weapon.AdsFOV;
    private GameSound GameSounds => weapon.WeaponSounds;

    private int bulletCount;
    private float firingTimer = 0;
    private bool isFiring = false;
    private bool isTriggerOn = false;
    private bool isReloading = false;
    private bool fired = false;
    private int bulletFired = 0;
    private int bulletsOnMag;
    private Vector3 targetPoint;
    private bool isInspecting;
    private bool IsSlideOut;
    private float timeToFireAgain => 1 / FireRate;// bullet/s
    private bool isAbleToShootAgain => firingTimer <= 0;
    private bool CanReload => bulletsOnMag < MagSize && bulletCount > 0 && !isReloading;
    public static event Action<AudioData> currentItemSoundEvent;
    private CinemachineImpulseSource impulseSource;
    private AudioData gunAudioData;
    private PlayerStateMachine playerStateMachine => PlayerStateMachine.instance;
    private Recoil recoil;
    public bool IsTriggerOn => isTriggerOn;
    public bool isAds { get; private set; }

    private void OnEnable()
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

    public override void Awake()
    {
        base.Awake();
        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();
    }
    private void Start()
    {
        bulletCount = BulletCount;
        bulletsOnMag = MagSize;

    }
    private void Update()
    {
        InspectCheck();
        FireCheck();
        AdsCheck();
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
    }
    void AdsCheck()
    {
        if (isAds)
        {
            playerStateMachine.FPS_VCamera.m_Lens.FieldOfView = Mathf.Lerp(playerStateMachine.FPS_VCamera.m_Lens.FieldOfView, (float)AdsFOV, 10 * Time.deltaTime);
        }
        else
        {
            playerStateMachine.FPS_VCamera.m_Lens.FieldOfView = Mathf.Lerp(playerStateMachine.FPS_VCamera.m_Lens.FieldOfView, 60, 10 * Time.deltaTime);
        }
    }
    void InspectCheck()
    {
        if (isInspecting)
        {
            if (playerStateMachine.PlayerState != PlayerState.Idle || isAds)
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
    void FireCheck()
    {
        if (!canFire) return;
        if ((!isFiring && fired || IsAuto && isAbleToShootAgain) && !isReloading && isTriggerOn)
        {
            if (IsBurst)
            {

                IEnumerator Wait(float time)
                {
                    for (int i = 0; i < BurstBulletCount; i++)
                    {
                        Shoot();
                        yield return new WaitForSeconds(time);
                    }
                }
                StartCoroutine(Wait(timeToFireAgain));

            }
            else
            {
                Shoot();
            }
        }
    }
    public override void OnActive()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnInactive()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnInteract() 
    {
        
    }
    public  void Inspect()
    {
        isInspecting = true;
    }
    public  void Reload()
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
        else if (bulletCount == 0)
        {
            // player Need Ammo
        }
    }
    void StartReloading()
    {
        ResetShoot();
        isReloading = true;
    }
    void applyReload()
    {
        // applied using animation event
        int bulletsToLoad = MagSize - bulletsOnMag;
        int bulletsNeeded = bulletsToLoad < bulletCount ? bulletsToLoad : bulletCount;
        bulletsOnMag += bulletsNeeded;
        bulletCount -= bulletsNeeded;
        isReloading = false;
        IsSlideOut = false;
        
    }

    public  void Fire(bool ctx)
    {
        isTriggerOn = ctx;
        fired = true;
    }
    public  void Aim(bool ctx)
    {
        playerStateMachine.IsAdsing = ctx;
        isAds = ctx;
    }
    void Shoot()
    {
        
        fired = false;
        if (bulletsOnMag > 0)
        {

            PlaySound(eWeaponStates.FireStartState);
            FiringInstantiation();
            PlayFiringShake();
            PlayFiringEffect();
            PlayFiringAnimation();
            if (recoil != null)
            {
            recoil.FireRecoil();
            }
            isFiring = true;
            bulletFired++;
            bulletsOnMag--;
            isInspecting = false;
            firingTimer = timeToFireAgain;

        }
        else
        {
            // TODO : notify player that he needs ammo
        }

    }
    void FiringInstantiation()
    {
        // Finalizing the Firing  calculating direcftion using Ray and instantiating bullet
        float firingCount = IsShotGun ? PelletsCount : 1;
        float spreadAngle = IsShotGun && isAds ? AimConeAngle / 2 : IsShotGun ? AimConeAngle : AimConeAngle / 10;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(MaxFiringDistance);
        }

        Vector3 directionWithoutSpread = targetPoint - firingOrigin.position;

        for (int i = 0; i < firingCount; i++)
        {
            Vector3 directionWithSpread = RandomDirectionWithinCone(directionWithoutSpread, spreadAngle);
            GameObject currentBullet = Instantiate(Bullet, firingOrigin.position, firingOrigin.rotation);
            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(directionWithSpread.normalized * BulletSpeed, ForceMode.Impulse);
        }
    }
    void PlayFiringShake()
    {
        // Calculate Shake Velocity and Appliying it
        float finalShakePower = IsAuto ? ShakePower / 2 : ShakePower;
        Vector3 shakeVelocity = new Vector3(0.1f, Camera.main.transform.position.y, Camera.main.transform.position.z)* finalShakePower/100;
        impulseSource.GenerateImpulseWithVelocity(shakeVelocity);
    }
    void PlayFiringEffect()
    {
        // Playing Effects
        muzzleFlash.SetActive(true);
        bulletShell.SetActive(true);
    }
    void PlayFiringAnimation()
    {
        // This Function Responsible of Playing Sync Firing Animation of Both Hands And Weapon
        Arm_animatorbrain.AnimatorBrainStopCurrent(this.itemLayer);
        AnimatorBrainStopCurrent(0);
        if (isAds)
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
    }
    Vector3 RandomDirectionWithinCone(Vector3 forward, float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad; // Convert Angle to Radiant
        float u = UnityEngine.Random.value; // Get Random Value 0 to 1
        float v = UnityEngine.Random.value; // Get Random Value 0 to 1
        float theta = 2 * Mathf.PI * u; // Get Random Theta using u
        float phi = Mathf.Acos(1 - v * (1 - Mathf.Cos(angleInRadians))); // Getting Random Phi Within Angle 
        // Spherical Cords Conversion
        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = Mathf.Cos(phi);

        Quaternion rotation = Quaternion.LookRotation(forward);

        Vector3 direction = rotation * new Vector3(x, y, z);

        return direction.normalized;
    }
    void ResetShoot()
    {
        isFiring = false;
        muzzleFlash.SetActive(false);
        bulletShell.SetActive(false);
        fired = false;
    }

    // Called By Animation Event
    void PlaySound(eWeaponStates weaponState)
    {
        if (audioMute) return;
        gunAudioData = GameSounds.GetSoundClip(weaponState, audioSource);
        currentItemSoundEvent?.Invoke(gunAudioData);
    }

    protected override void OnMouseOver()
    {
    }
}
