using UnityEngine.InputSystem;
using UnityEngine;

public class Door : InteractableBase
{
    bool isOpen = false;
    public override void InteractCompleted()
    {
        OpenClose();
    }

    void OpenClose()
    {
        isOpen = !isOpen;
        float angle = isOpen ? -90 : 0;
        transform.parent.rotation = Quaternion.Euler(Vector3.up * angle);
    }
    
}
