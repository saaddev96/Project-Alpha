using System.Collections;
using UnityEngine;
public abstract class Weapon : Item
{


    public override void OnActive()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnInactive()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnInteract() 
    {
        
    }
    public virtual void Inspect()
    {

    }
    public virtual void Reload()
    {

    }
    public virtual void Fire()
    {

    }
    public virtual void Aim(bool ctx)
    {

    }

    protected abstract override void OnMouseOver();

 

}
