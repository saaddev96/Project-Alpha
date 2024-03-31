using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader")]
public class InputReader : ScriptableObject, CustomInputs.IGamePlayActions
{
    private CustomInputs _customIUnputs;

    public event Action<Vector2> MoveEvent;
    public event Action MoveCanceledEvent;
    public event Action<Vector2> LookEvent;
    public event Action JumpEvent;
    public event Action JumpCancelledEvent;
    public event Action SprintEvent;
    public event Action SprintCancelledEvent;
    private void OnEnable()
    {
        if(_customIUnputs== null)
        {
            _customIUnputs = new CustomInputs();
            _customIUnputs.GamePlay.SetCallbacks(this);
            SetGameplayInputs();
        }
    }

    void SetGameplayInputs()
    {
        _customIUnputs.GamePlay.Enable();
    }
    void ClearGameplayInputs()
    {
        _customIUnputs.GamePlay.Disable();
    }
    private void OnDisable()
    {
        if (_customIUnputs != null)
            ClearGameplayInputs();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
        if(context.phase == InputActionPhase.Canceled)
        {
            MoveCanceledEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
        JumpEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            JumpCancelledEvent?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SprintEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            SprintCancelledEvent?.Invoke();
        }

    }
}
