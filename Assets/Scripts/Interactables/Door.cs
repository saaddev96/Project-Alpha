using UnityEngine.InputSystem;
using UnityEngine;

public class Door : InteractableBase
{

    public override void OnInteract()
    {
        elapsedTime = 0;
        fillAmount = 0;
        timeToInteract = 3.0f;
        interactableName = "Door";
        UiManager.Instance.InteractBG.gameObject.SetActive(true);
        UiManager.Instance.InteractFill.fillAmount = fillAmount;
        playerStateMachine.IsInteracting = true;
    }


}
