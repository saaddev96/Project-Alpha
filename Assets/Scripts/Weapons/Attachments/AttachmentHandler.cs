using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum AttachmentType { None = 0, Sight, Muzzle, Tactical, FrontGrip, RearGrip, Stock }
public class AttachmentHandler : MonoBehaviour
{
    [Header("Weapon Attachments")]
    [SerializeField] private WeaponAttachment[] weaponAttachments;


    // attachment references 
    // index zero should always be 0 make sure to sit it in inspector
    private Attachment currentSight;
    private Attachment currentMuzzle;
    private Attachment CurrentSight { get {return currentSight; } set { currentSight = value; } }
    private Attachment CurrentMuzzle { get { return currentMuzzle; } set { currentMuzzle = value; } }

    Weapon weapon;
    Vector3 DesiredOffset;
    private void Awake()
    {
        weapon = this.gameObject.GetComponent<Weapon>();
    }
    private void Start()
    {
        RemoveAllAttachments();
    }

    [Serializable]
    public struct WeaponAttachment
    {
        public AttachmentType type;
        public Attachment[] attachments;
    }

    void RemoveAllAttachments()
    {
        CurrentSight = GetAttachment(AttachmentType.Sight, WeaponAttachments.IronSight);
        CurrentMuzzle = GetAttachment(AttachmentType.Muzzle, WeaponAttachments.EmptyMuzzle);
        CurrentSight.Initialize(weapon);
        CurrentMuzzle.Initialize(weapon);
    }
    Attachment GetAttachment(AttachmentType type,WeaponAttachments attachment)
    {
        WeaponAttachment WeaponAttachment = Array.Find(weaponAttachments, x => 
        {
            return x.type == type;
        });
        Attachment _attachment = Array.Find(WeaponAttachment.attachments, x => x.W_attachment == attachment);
        return _attachment;
    }

    public void ChangeAttachment(AttachmentType type, WeaponAttachments attachment)
    {
        switch (type)
        {
            case AttachmentType.Muzzle:
                CurrentMuzzle.DisableAttachment();
                CurrentMuzzle = GetAttachment(type, attachment);
                CurrentMuzzle.Initialize(weapon);
                break;
            case AttachmentType.Sight:
                CurrentSight.DisableAttachment();
                CurrentSight = GetAttachment(type, attachment);
                CurrentSight.Initialize(weapon);
                break;

        }

    }

    public void ChangeToNext(AttachmentType type)
    {
        
        switch (type)
        {
            case AttachmentType.Muzzle:
                WeaponAttachment MuzzleAttachment = Array.Find(weaponAttachments, x =>
                {
                    return x.type == type;
                });
                int muzzleCurrentIndex = Array.IndexOf(MuzzleAttachment.attachments, CurrentMuzzle) + 1;
                muzzleCurrentIndex %=MuzzleAttachment.attachments.Length;
                CurrentMuzzle.DisableAttachment();
                CurrentMuzzle = MuzzleAttachment.attachments[muzzleCurrentIndex];
                CurrentMuzzle.Initialize(weapon);
                break;
            case AttachmentType.Sight:
                WeaponAttachment SightAttachment = Array.Find(weaponAttachments, x =>
                {
                    return x.type == type;
                });
                int SightCurrentIndex = Array.IndexOf(SightAttachment.attachments, CurrentSight) + 1;
                SightCurrentIndex %= SightAttachment.attachments.Length;
                CurrentSight.DisableAttachment();
                CurrentSight = SightAttachment.attachments[SightCurrentIndex];
                CurrentSight.Initialize(weapon);
                break;
        }

    }
   
  
}
