using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{

    public void OnInteracterEnter();
    public void OnInteracterExit();
    public void OnInteract();
}
