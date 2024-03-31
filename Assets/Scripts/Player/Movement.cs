using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Functional Parameters")]
    [SerializeField] private bool canSprinte;
    [SerializeField] private bool canSlope;
    [SerializeField] private bool canCrouch;
    [SerializeField] private bool canJump;
    [Header("PLAYER INPUT READER")]
    [SerializeField] private InputReader _inputReader;
    [Space]

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [Space]

    [Header("Jump Parameters")]
    [SerializeField] private float JumpHeight;
    [SerializeField] private float gravityMultiplier =-8.9f;
    [SerializeField] private float windPower = 1.4f;
    [Space]

    [Header("Slope Parameters")]
    [SerializeField] private float slopeSpeed;
    [Space]

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator FPS_Animator;
    CharacterController _characterController;
    Vector2 inputMoveDirection;
    bool isGrounded =>_characterController.isGrounded;
    bool isJumping= false;
    bool isSprinting = false;
    bool isMoving = false;
    [SerializeField] bool isAirBorn = false;
    Vector3 moveDirection;
    Vector3 hitnormal;

    public bool IsPlayerMoving => isMoving;
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
        }
        if (moveDirection.y < -1f)
        {
            isAirBorn = true;
            if (Physics.SphereCast(transform.position, 1.1f, Vector3.down, out _, 0.8f, groundMask)&& isAirBorn)
            {
                FPS_Animator.SetBool("isLanding", isAirBorn);
                isAirBorn = false;
            }

        }
        else
        {
            isAirBorn = false;
            FPS_Animator.SetBool("isLanding", isAirBorn);
        }

    }
  
}
