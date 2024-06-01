using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silencer : Attachment
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
        _weapon.HasSilencer = true;
    }
    public override void DisableAttachment()
    {
        this.gameObject.SetActive(false);
        _weapon.HasSilencer = false;
    }
}
