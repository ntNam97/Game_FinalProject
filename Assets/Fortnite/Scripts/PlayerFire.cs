using UnityEngine;
using UnityEngine.Networking;

public class PlayerFire : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";
    public PlayerWeapon weapon;
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;

    void Start()
    {
        cam = GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<Camera>();
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if (_hit.rigidbody)
            {
                Debug.Log("_hit.rigidbody.tag: " + _hit.rigidbody.tag);
                if (_hit.rigidbody.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(_hit.rigidbody.name);
                }
            }


            // We hit something, call the OnHit method on the server
        }
    }

    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + " has been shot");
    }

}
