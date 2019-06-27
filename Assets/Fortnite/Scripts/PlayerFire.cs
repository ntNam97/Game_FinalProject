using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(WeaponManager))]
public class PlayerFire : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    WeaponSwitching weaponSwitching;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    void Start()
    {
        cam = GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }
    void Update()
    {
    
        currentWeapon = weaponManager.CurrentWeapon;
        if (weaponManager!= null)
            Debug.Log("weaponManager not null");
        else
            Debug.Log("weaponManager null");

        if (currentWeapon.fireRate<=0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
       
    }

    [Client]
    void Shoot()
    {
        Debug.Log("SHOOT");
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider)
            {
                Debug.Log("hit "+ _hit.collider.tag);
                if (_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
                }
            }


            // We hit something, call the OnHit method on the server
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID,int _dmg)
    {
        Debug.Log(_playerID + " has been shot");
        PlayerManager player = GameManager.getPlayer(_playerID);
        player.RpcTakeDamage(_dmg);
    }

}
