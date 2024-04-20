using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputReader : MonoBehaviour
{
    public event Action<Vector2> MoveEvent;
    public event Action MoveCanceledEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> JumpEvent;
    public event Action<bool> SprintEvent;
    public event Action<bool> CrouchEvent;
    public event Action PauseEvent;
    public event Action<bool> InteractEvent;

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
        JumpEvent?.Invoke(context.ReadValueAsButton());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintEvent?.Invoke(context.ReadValueAsButton());
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchEvent?.Invoke(context.ReadValueAsButton());
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            InteractEvent?.Invoke(false);
        }
    }
}
