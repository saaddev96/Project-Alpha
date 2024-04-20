using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    protected float timeToInteract;
    protected string interactableName;
    protected float elapsedTime;
    protected float fillAmount;
    protected PlayerStateMachine playerStateMachine;
    public void Awake()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }
    public abstract void OnInteract();
    public void OnInteracterEnter()
    {
        UiManager.Instance.InteractText.gameObject.SetActive(true);
        string startString = PlayerStateMachine.Playerinput.currentActionMap.FindAction("Interact").bindings[0].path;
        UiManager.Instance.InteractText.text = $"Press {startString.Split("/").GetValue(1).ToString().ToUpper()} to Interact";
    }
    public void OnInteracterExit()
    {
        UiManager.Instance.InteractText.text = "";
        UiManager.Instance.InteractText.gameObject.SetActive(false);
    }
    public void OnInteracting()
    {
        elapsedTime += Time.deltaTime;
        fillAmount = Mathf.Lerp(0, 1, elapsedTime / timeToInteract);
        string progress = $"{Mathf.RoundToInt(fillAmount * 100)}%";
        UiManager.Instance.InteractFill.fillAmount = fillAmount;
        UiManager.Instance.InteractTextCounter.text = progress;
        OnInteractingFinished();
    }
    void OnInteractingFinished()
    {
        if (fillAmount >= 0.99f)
        {
            OnInteractingExit();
   
        }
    }

    public void OnInteractingExit()
    {
        playerStateMachine.IsInteracting = false;
        UiManager.Instance.InteractBG.gameObject.SetActive(false);
    }
}
