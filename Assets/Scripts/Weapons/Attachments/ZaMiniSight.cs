using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaMiniSight : Attachment
{
    [SerializeField] private Transform sightOrigin;
    private Vector3 desiredOffsetY = new(0f, 0.01f,0f);
    public override void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        EnableAttachment();
    }
    public override void EnableAttachment()
    {
        this.gameObject.SetActive(true);
        _weapon.SightOrigin = sightOrigin;
        transform.parent.gameObject.SetActive(true);
        _weapon.AlignSight(-desiredOffsetY);

    }
    public override void DisableAttachment()
    {
        this.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        _weapon.AlignSight(desiredOffsetY);
    }
    // Calculate y offset for aligning hands
    //Vector3 CalculateOffset()
    //{
    //    Vector3 offset = transform.parent.position - sightOrigin.position;
    //    offset = new Vector3(0, offset.y, 0);
    //    print(offset.y);
    //    return offset;
    //}
}
