
public abstract class Weapon : Item
{

    protected abstract override void OnActive();

    protected abstract override void OnInactive();

    protected abstract override void OnInteract();

    protected abstract override void OnMouseOver();
}
