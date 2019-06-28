using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    private bool isReloading = false;
    public PlayerWeapon CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }

        set
        {
            currentWeapon = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        primaryWeapon.setDefault();
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns =(GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponentInChildren<WeaponGraphics>();
        if (currentGraphics == null)
            Debug.LogError("No WeaponGraphics component on the weapon object: " + _weaponIns.name);

        if (isLocalPlayer)
            _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
    }
    public void Reload()
    {
        if (isReloading)
            return;
        isReloading = true;
        currentWeapon.bullets = currentWeapon.maxBullets;
        isReloading = false;
    }
    private IEnumerator Reload_Coroutine()
    {
        Debug.Log("Reloading...");

        isReloading = true;

        //CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.bullets = currentWeapon.maxBullets;

        isReloading = false;
    }
    //[Command]
    //void CmdOnReload()
    //{
    //    RpcOnReload();
    //}

    //[ClientRpc]
    //void RpcOnReload()
    //{
    //    Animator anim = currentGraphics.GetComponent<Animator>();
    //    if (anim != null)
    //    {
    //        anim.SetTrigger("Reload");
    //    }
    //}
}
