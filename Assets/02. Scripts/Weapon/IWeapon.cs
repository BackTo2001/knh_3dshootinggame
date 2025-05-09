public interface IWeapon
{
    public void Initialize();
    public void Attack();
    public void Reload();
    public void CancelReload();
    public bool IsReloading { get; }
}
