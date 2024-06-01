using UnityEngine;

public enum WeaponAttachments {None =0, PiladSight,IronSight,Silencer,EmptyMuzzle}
public abstract class Attachment : MonoBehaviour
{
    [SerializeField] private WeaponAttachments w_attachment = WeaponAttachments.None;
    public WeaponAttachments W_attachment => w_attachment;
    public abstract void Initialize(Weapon weapon);
    public abstract void EnableAttachment();
    public abstract void DisableAttachment();
}
