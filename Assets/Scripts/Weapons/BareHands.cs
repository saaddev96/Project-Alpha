using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BareHands : Item
{
    private void OnEnable()
    {
    }
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

    protected override void OnMouseOver()
    {
    }


}
