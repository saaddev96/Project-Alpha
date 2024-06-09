using UnityEngine;

public enum WeaponAttachments {None =0,ZaMiniSight,IronSight,Silencer,EmptyMuzzle}
public abstract class Attachment : MonoBehaviour
{
    [SerializeField] private WeaponAttachments w_attachment = WeaponAttachments.None;
    public WeaponAttachments W_attachment => w_attachment;
    protected  Weapon _weapon;
    public abstract void Initialize(Weapon weapon);
    public abstract void EnableAttachment();
    public abstract void DisableAttachment();

 
}
