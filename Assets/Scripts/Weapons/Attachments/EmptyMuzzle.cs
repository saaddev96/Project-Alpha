using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMuzzle : Attachment
{
    private Weapon _weapon;
    public override void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        EnableAttachment();
    }

    public override void EnableAttachment()
    {
        this.gameObject.SetActive(true);
    }
    public override void DisableAttachment()
    {
        this.gameObject.SetActive(false);
    }
}
