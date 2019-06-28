using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(WeaponManager))]
public class PlayerFire : NetworkBehaviour
{
    [SerializeField]
    private Transform weaponHolder;
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    WeaponSwitching weaponSwitching;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon=null;
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
        
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (PauseMenu.IsOn)
            return;
        if (weaponManager!= null)
            Debug.Log("weaponManager not null");
        else
            Debug.Log("weaponManager null");
        if (currentWeapon != null)
            Debug.Log("currentWeapon not null");
        else
            Debug.Log("currentWeapon null");

        if (currentWeapon.fireRate<=0f)
        {
            Animator anim = weaponHolder.GetComponentInChildren<Animator>();
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
              
            }
            if (Input.GetButtonUp("Fire1"))
            {
               
                anim.SetBool("bFire", false);
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
        if(!isLocalPlayer)
        {
            return;
        }
        if(currentWeapon.bullets<=0)
        {
            //weaponManager.Reload();
            Debug.Log("out of bullets");
            return;
        }
        Animator anim = weaponHolder.GetComponentInChildren<Animator>();
        anim.SetBool("bFire", true);
        currentWeapon.bullets--;
        Debug.Log("Remaining bullets " + currentWeapon.bullets);
        Debug.Log("SHOOT");
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if (_hit.collider)
            {
                Debug.Log("hit "+ _hit.collider.tag);
                if (_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage,GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name);
                    Debug.Log(GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name + " shoot " + _hit.collider.name);
                }
            }


            // We hit something, call the OnHit method on the server
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID,int _dmg,string _sourceID)
    {
        Debug.Log(_playerID + " has been shot");
        PlayerManager player = GameManager.getPlayer(_playerID);
        player.RpcTakeDamage(_dmg,_sourceID);
    }

}
