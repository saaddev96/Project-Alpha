
using UnityEngine;
using System;
public class Movement : AnimatorBrain
{

    public static Movement instance;

    [Header("Functional Parameters")]
    [SerializeField] private bool canSprinte;
    [SerializeField] private bool canSlope;
    [SerializeField] private bool canCrouch;
    [SerializeField] private bool canJump;
    [Space]
    [Header("Player Input Reader")]
    [SerializeField] private InputReader _inputReader;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [Space]

    [Header("Jump Parameters")]
    [SerializeField] private float JumpHeight;
    [SerializeField] private float gravityMultiplier =-8.9f;
    [SerializeField] private float windPower = 1.4f;
    [SerializeField] bool isAirBorn = false;
    [SerializeField] private AudioSource FootAudioSource;
    [Space]

    [Header("Slope Parameters")]
    [SerializeField] private float slopeSpeed;
    [Space]
    [Header("Raycast Layer")]
    [SerializeField] private LayerMask groundMask;

    [Header("Character Animator")]
    [SerializeField] private Animator FPS_Animator;
    // public readonly fields
    public bool IsPlayerMoving => isMoving;

    // private Field
    private CharacterController _characterController;
    private Vector2 inputMoveDirection;
    private bool isGrounded =>_characterController.isGrounded;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isMoving = false;
    private Vector3 moveDirection;
    private Vector3 hitnormal;
    private Texture ActiveTerrainTexture => Footsteps.ActiveTerrainTexture;
    
    // Events
    public static event Action<Data> OnPlayerLandedEvent;
    private bool isSliding
    {
        get
        {
            if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out RaycastHit slopHit, 1.5f, groundMask))
            {
                hitnormal = slopHit.normal;
                float angle = Vector3.Angle(hitnormal, Vector3.up);
                return angle > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    private  void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        AnimatorBrainInit(FPS_Animator.layerCount,FPS_Animator,eAnimation.Idle);
        if(_characterController == null)
        {
            _characterController = gameObject.GetComponent<CharacterController>();
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        _inputReader.MoveEvent += HandleMoveInput;
        _inputReader.JumpEvent += HandleJump;
        _inputReader.JumpCancelledEvent += HandleJumpCancel;
        _inputReader.SprintEvent += HandleSprint;
        _inputReader.SprintCancelledEvent += HandleSprintCancel;
        _inputReader.MoveCanceledEvent += MoveCanceled;

    }
    private void OnDisable()
    {
        _inputReader.MoveEvent -= HandleMoveInput;
        _inputReader.JumpEvent -= HandleJump;
        _inputReader.JumpCancelledEvent -= HandleJumpCancel;
        _inputReader.SprintEvent -= HandleSprint;
        _inputReader.SprintCancelledEvent -= HandleSprintCancel;
        _inputReader.MoveCanceledEvent -= MoveCanceled;
    }
    
    private void Update()
    {
        PlayerFinalMovement();
    }

    void HandleMoveInput(Vector2 dir)
    {
        isMoving = true;
        inputMoveDirection = dir;
    }
    void MoveCanceled()
    {
        isMoving = false;
    }
    void HandleJump()
    {
        isJumping = true;
    }
    void HandleJumpCancel()
    {
        isJumping = false;
    }
    void HandleSprint()
    {
        isSprinting = true;

    }
    void HandleSprintCancel()
    {
        isSprinting = false;

    }

    void PlayerFinalMovement()
    {
        if (isGrounded)
        {

            Move();
            Jump();
            Slide();
        }
        Land();
        _characterController.Move(moveDirection * Time.deltaTime);
    }
    void Move()
    {
  
        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        moveDirection = (transform.forward * inputMoveDirection.y * moveSpeed + transform.right * inputMoveDirection.x * moveSpeed * 0.7f);
        if (isMoving)
        {
            if (isSprinting)
            {
                AnimatorBrainPlay(eAnimation.Sprint,0,false,false);
            }
            else
            {
                AnimatorBrainPlay(eAnimation.Walk, 0, false, false);
            }
        }
        else
        {
            AnimatorBrainPlay(eAnimation.Idle, 0, false, false);
        }

    }
    void Jump()
    {
        if (isJumping && canJump)
        {
            AnimatorBrainPlay(eAnimation.Jump, 0, false, false);
            moveDirection.y = Mathf.Sqrt(JumpHeight / 10 * -2f * gravityMultiplier);
            if (isMoving)
                moveDirection -= transform.forward * windPower;
        }
    }
    void Slide()
    {
        if (isSliding && canSlope)
        {
            moveDirection = new Vector3(hitnormal.x, -hitnormal.y, hitnormal.z) * slopeSpeed;

        }
    }
    void Land()
    {
        if (!isGrounded)
        {
            moveDirection.y += gravityMultiplier * Time.deltaTime;
            if (moveDirection.y < -4.2)
            {
                isAirBorn = true;
            }
        }
      
        else if(isGrounded && isAirBorn)
        {
            AnimatorBrainPlay(eAnimation.Land, 0, true, false);
            string floorAudio;
            if (ActiveTerrainTexture != null && ActiveTerrainTexture.name.Contains("water"))
            {
                floorAudio = "WaterSplash";
            }
            else
            {
                floorAudio = "Land";
            }
            OnPlayerLandedEvent?.Invoke(new AudioData { clip = GameAssets.Instance.PlayerSounds.GetSoundClip(floorAudio), aSource = FootAudioSource, volume = 0.1f });
            isAirBorn = false;
        }
        else
        {
           
        }
    }
  
    
}
