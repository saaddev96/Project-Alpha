
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
    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchingSpeed = 2.5f;
    [SerializeField] private float timeToCrouch = 10f;
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
    private float playerHeight;
    private bool isGrounded =>_characterController.isGrounded;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isMoving = false;
    private bool isCrouching = false;
    private bool isCrouched = false;
    private Vector3 moveDirection;
    private Vector3 hitnormal;
    private Texture ActiveTerrainTexture => Footsteps.ActiveTerrainTexture;

    //Constants
    private const float groundCheckDistance = 1.5f;
    private const float upwardCheckDistance = 1f;
    // Events
    public static event Action<Data> OnPlayerLandedEvent;
    float angle;
    private bool isSliding
    {
        get
        {
            if (Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out RaycastHit slopHit,groundCheckDistance, groundMask))
            {
                hitnormal = slopHit.normal;
                angle = Vector3.Angle(hitnormal, Vector3.up);
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
        if(_characterController == null)
        {
            _characterController = gameObject.GetComponent<CharacterController>();
        }
        AnimatorBrainInit(FPS_Animator.layerCount,FPS_Animator,eAnimation.Idle);
        playerHeight = _characterController.height;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        _inputReader.MoveEvent += HandleMoveInput;
        _inputReader.MoveCanceledEvent += MoveCanceledInput;
        _inputReader.JumpEvent += HandleJumpInput;
        _inputReader.JumpCancelledEvent += HandleJumpCancelInput;
        _inputReader.SprintEvent += HandleSprintInput;
        _inputReader.SprintCancelledEvent += HandleSprintCancelInput;
        _inputReader.CrouchEvent += HandleCrouchInput;
        _inputReader.CrouchCancelledEvent += HandleCrouchCancelInput;

    }
    private void OnDisable()
    {
        _inputReader.MoveEvent -= HandleMoveInput;
        _inputReader.MoveCanceledEvent -= MoveCanceledInput;
        _inputReader.JumpEvent -= HandleJumpInput;
        _inputReader.JumpCancelledEvent -= HandleJumpCancelInput;
        _inputReader.SprintEvent -= HandleSprintInput;
        _inputReader.SprintCancelledEvent -= HandleSprintCancelInput;
        _inputReader.CrouchEvent -= HandleCrouchInput;
        _inputReader.CrouchCancelledEvent -= HandleCrouchCancelInput;
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
    void MoveCanceledInput()
    {
        isMoving = false;
    }
    void HandleJumpInput()
    {
        isJumping = true;
    }
    void HandleJumpCancelInput()
    {
        isJumping = false;
    }
    void HandleSprintInput()
    {
        isSprinting = true;

    }
    void HandleSprintCancelInput()
    {
        isSprinting = false;

    }
    void HandleCrouchInput()
    {
        isCrouching = true;

    }
    void HandleCrouchCancelInput()
    {
        isCrouching = false;
    }

    void PlayerFinalMovement()
    {
        if (isGrounded)
        {
            Move();
            Jump();
            Slide();
        }
        Crouch();
        Land();
        _characterController.Move(moveDirection * Time.deltaTime);
    }
    float moveSpeed;
    void Move()
    {
  
        float moveSpeed = isCrouched ? crouchingSpeed : isSprinting ? sprintSpeed :walkSpeed;
        moveDirection = (transform.forward * inputMoveDirection.y * moveSpeed+ transform.right * inputMoveDirection.x * moveSpeed * 0.7f+Vector3.down*_characterController.height/2*slopeSpeed);
    
        if (isMoving)
        {
            if (isCrouched)
            {
                AnimatorBrainPlay(eAnimation.Crouch, 0, false, false);
            }
            else if (isSprinting && !isCrouched)
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
            if(!CheckAbovePlayer(out RaycastHit hit))
            {
            isJumping = false;
            AnimatorBrainPlay(eAnimation.Jump, 0, false, false);
            moveDirection.y = Mathf.Sqrt(JumpHeight / 10 * -2f * gravityMultiplier);
            if (isMoving)
                moveDirection -= transform.forward * windPower;
            }
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
            AnimatorBrainPlay(eAnimation.Land, 0, true, false,0.2f);
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
    }

    void Crouch()
    {

        if (isCrouching && canCrouch)
        {
            isCrouched = true;
            if (_characterController.height > crouchHeight)
            {
                _characterController.height = Mathf.Lerp(_characterController.height, crouchHeight, timeToCrouch * Time.deltaTime);
            }

        }
        else
        {

            if (_characterController.height < playerHeight)
            {
                if (!CheckAbovePlayer(out RaycastHit hit))
                {
                    isCrouched = false;
                    _characterController.height = Mathf.Lerp(_characterController.height, playerHeight, timeToCrouch * Time.deltaTime);
                }
            }
        }
    }
    bool CheckAbovePlayer(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, Vector3.up, out hit, upwardCheckDistance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up); ;
    }
}
