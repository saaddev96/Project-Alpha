
using UnityEngine;
using System;
public class Movement : MonoBehaviour
{
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
    [Space]
    [Header("Character Animator")]
    [SerializeField] private Animator FPS_Animator;

    // public Read only field 
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

    // Events
    public static event Action<Data> OnPlayerLandedEvent;
    private bool isSliding
    {
        get
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopHit, 1.5f, groundMask))
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

    private void Awake()
    {
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
        FPS_Animator.SetBool("isMoving", isMoving);
    }
    void MoveCanceled()
    {
        isMoving = false;
        FPS_Animator.SetBool("isMoving", isMoving);
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
        FPS_Animator.SetInteger("Speed", (int)sprintSpeed);
    }
    void HandleSprintCancel()
    {
        isSprinting = false;
        FPS_Animator.SetInteger("Speed", (int)walkSpeed);
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
        
    }
    void Jump()
    {
        if (isJumping && canJump)
        {
            FPS_Animator.Play("Jump", 0);
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
            FPS_Animator.SetBool("isLanding", isAirBorn);
            OnPlayerLandedEvent?.Invoke(new AudioData {clip=GameAssets.Instance.PlayerSounds.GetSoundClip("Land"), aSource=FootAudioSource,volume=0.1f });
            isAirBorn = false;
        }
        else
        {
            FPS_Animator.SetBool("isLanding", false);
        }
    }
  
}
