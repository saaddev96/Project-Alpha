using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
public enum eAnimation
{
    Idle,
    Walk,
    Sprint,
    Crouch,
    Jump,
    Land,
    Interact
}
public enum PlayerState
{
    None,
    Idle,
    Walking,
    Jumping,
    Sliding,
    Crouching,
    Interacting
}
public class PlayerStateMachine : AnimatorBrain<eAnimation>
{
    private PlayerBaseState _currentState;
    private StateFactory _states;

    public static PlayerStateMachine instance;
    [Header("Player State")]
    [SerializeField]private PlayerState playerState = PlayerState.None;
    [Space]
    [Header("Functional Parameters")]
    [SerializeField] private bool canSprinte;
    [SerializeField] private bool canSlope;
    [SerializeField] private bool canCrouch;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canInteract;
    [Space]
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [Space]

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityMultiplier = -8.9f;
    [SerializeField] bool isAirBorn = false;
    [SerializeField] private AudioSource feetAudioSource;
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
    [SerializeField] private LayerMask interactionMask;

    [Header("Character Animator")]
    [SerializeField] private Animator FPS_Animator;
    // public readonly fields
    public bool IsPlayerMoving => isMoving;
    public Vector3 moveDirection;

    // private Field
    private CharacterController _characterController;
    private InputReader _inputReader;
    private static PlayerInput _playerinput;
    private Vector2 inputMoveDirection;
    private float angle;
    private string floorAudio;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isMoving = false;
    private bool isCrouching = false;
    private bool isCrouched = false;
    private bool interacted = false;
    private bool ableToInteract = false;
    private bool isInteracting = false;
    private Vector3 groundCheckHitnormal;
    private Vector3 groundCheckHitPoint;
    private AudioData LandAudioData = new AudioData();
    private float moveSpeed;
    private float playerHeight;
    //Constants
    private const float upwardCheckDistance = 1f;
    // Events
    public static event Action<Data> OnPlayerLandedEvent;
    // static 
    public  InteractableBase currentInteractable;
    public bool IsSliding
    {
        get
        {
            if (Physics.Raycast(transform.position,Vector3.down, out RaycastHit slopHit,2f, groundMask))
            {
                groundCheckHitnormal = slopHit.normal;
                groundCheckHitPoint = slopHit.point;
                angle = Vector3.Angle(groundCheckHitnormal, Vector3.up);
                return angle > _characterController.slopeLimit && isGrounded && angle<80;

            }
            else
            {
                return false;
            }
        }
    }
    public PlayerBaseState CurrentState {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }
    public Texture ActiveTerrainTexture => Footsteps.ActiveTerrainTexture;
    public bool isGrounded => _characterController.isGrounded;
    public bool CanJump { get { return canJump; } set { canJump = value; } }
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public bool IsMoving => isMoving;
    public bool IsCrouching => isCrouching;
    public bool CanCrouch => canCrouch;
    public float TimeToCrouch => timeToCrouch;
    public float CrouchHeight => crouchHeight;
    public bool IsSprinting => isSprinting;
    public bool Interacted => interacted;
    public bool CanInteract => canInteract;
    public bool AbleToInteract => ableToInteract;
    public bool IsInteracting
    {
        get
        {
            return isInteracting;
        }
        set
        {
            isInteracting = value;
        }
    }
    public float WalkSpeed => walkSpeed;
    public float SprintSpeed => sprintSpeed;
    public float SlopeSpeed => slopeSpeed;
    public float JumpHeight => jumpHeight;
    public float PlayerHeight => playerHeight;
    public float GravityMultiplier => gravityMultiplier;
    public Vector2 InputMoveDirection => inputMoveDirection;
    public CharacterController CC { get
        {
            return _characterController;
        }
        set
        {
            _characterController = value;
        }
    }
    public Vector3 MoveDirection
    {
        get
        {
            return moveDirection;
        }
        set
        {
            moveDirection = value;
        }
    }
    public AudioSource FeetAudioSource => feetAudioSource;
    public Vector3 GroundCheckHitnormal => groundCheckHitnormal;
    public Vector3 GroundCheckHitpoint => groundCheckHitPoint;
    public float MoveSpeed => moveSpeed;
    public bool IsAirBorn { 
        get
        {
            return isAirBorn;
        }
        set
        {
            isAirBorn = value;
        }
    }
    public bool IsCrouched
    {
        get { return isCrouched; }
        set { isCrouched = value; }
    }
    public PlayerState PlayerState { get { return playerState; }set { playerState = value; } }
    public static PlayerInput Playerinput => _playerinput;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (_characterController == null)
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            playerHeight = _characterController.height;
        }
        _inputReader = GetComponent<InputReader>();
        _playerinput = GetComponent<PlayerInput>();
        AnimatorBrainInit(FPS_Animator.layerCount, FPS_Animator, eAnimation.Idle);
        _states = new StateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }
    private void OnEnable()
    {
        _inputReader.MoveEvent += HandleMoveInput;
        _inputReader.MoveCanceledEvent += MoveCanceledInput;
        _inputReader.JumpEvent += HandleJumpInput;
        _inputReader.SprintEvent += HandleSprintInput;
        _inputReader.CrouchEvent += HandleCrouchInput;
        _inputReader.InteractEvent += HandleInteract;

    }
    private void OnDisable()
    {
        _inputReader.MoveEvent -= HandleMoveInput;
        _inputReader.MoveCanceledEvent -= MoveCanceledInput;
        _inputReader.JumpEvent -= HandleJumpInput;
        _inputReader.SprintEvent -= HandleSprintInput;
        _inputReader.CrouchEvent -= HandleCrouchInput;
        _inputReader.InteractEvent -= HandleInteract;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Land();
        HandleInteract();
        _currentState.UpdateStates();
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
    void HandleJumpInput(bool ctx)
    {
        isJumping = ctx;
    }

    void HandleSprintInput(bool ctx)
    {
        isSprinting = ctx;

    }
    void HandleCrouchInput(bool ctx)
    {
        isCrouching = ctx;
    }

    void HandleInteract(bool ctx)
    {
        interacted = ctx;
    }
    public void FPSControl()
    {
        moveSpeed = isCrouched ? crouchingSpeed : isSprinting ? sprintSpeed : walkSpeed;
        moveDirection = (transform.forward * inputMoveDirection.y * moveSpeed + transform.right * inputMoveDirection.x * moveSpeed * 0.7f + Vector3.down * _characterController.height / 2 * slopeSpeed);
    }
  
    public void Land()
    {
        if (!isGrounded)
        {

            moveDirection.y += gravityMultiplier * Time.deltaTime;
            if (moveDirection.y < -5.9)
            {
                
                isAirBorn = true;
            }

        }

        else if (isGrounded && isAirBorn)
        {
            isJumping = false;
            AnimatorBrainPlay(eAnimation.Land, 0, true, false, 0.2f);
            if (ActiveTerrainTexture != null && ActiveTerrainTexture.name.Contains("water"))
            {
                floorAudio = "WaterSplash";
            }
            else
            {
                floorAudio = "Land";
            }
            LandAudioData.clip = GameAssets.Instance.PlayerSounds.GetSoundClip(floorAudio);
            LandAudioData.aSource = feetAudioSource;
            LandAudioData.volume = 0.1f;
            OnPlayerLandedEvent?.Invoke(LandAudioData);
            isAirBorn = false;
        }
    }
    void HandleInteract()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit ,5f, interactionMask))
        {
            if (hit.collider.TryGetComponent<InteractableBase>(out currentInteractable)&&!isInteracting)
            {
                currentInteractable.OnInteracterEnter();
                ableToInteract = true;
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnInteracterExit();
                ableToInteract = false;
            }
        }
     
    }
    public  bool CheckAbovePlayer(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, Vector3.up, out hit, upwardCheckDistance);
    }

 
}
