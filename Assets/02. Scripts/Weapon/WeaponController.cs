using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private IWeapon _currentWeapon;

    public void EquipWeapon(IWeapon weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.Initialize();
    }

}
