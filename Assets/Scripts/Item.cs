using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public  string itemName;

    protected abstract void OnMouseOver();
    protected abstract void OnInteract();
    protected abstract void OnActive();
    protected abstract void OnInactive();

}
