using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(UnityStandardAssets.Characters.FirstPerson.FirstPersonController))]
public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayer = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    
    [HideInInspector]
    public GameObject playerUIInstance;
	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            DisableComponents();
            gameObject.transform.Find("BRS_UI").GetComponentInChildren<Canvas>().enabled = false;
            gameObject.transform.Find("Canvas").GetComponent<Canvas>().enabled = false;
            GetComponent<PlayerFire>().enabled = false;
            GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<WeaponSwitching>().enabled = false;
            Debug.Log("isNotLocal");
            GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
            GetComponentInChildren<CharacterController>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
           

        }
        else
        {

            playerGraphics.layer = LayerMask.NameToLayer(dontDrawLayer);
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if(!ui)
            {
                Debug.Log("No UI COMPONENT IN PREFAB");
            }
            ui.setPlayer(GetComponent<PlayerManager>());
            GetComponent<PlayerManager>().PlayerSetup();
            string _username = "Loading...";
            if (UserAccountManager.IsLoggedIn)
                _username = UserAccountManager.LoggedIn_Username;
            else
                _username = GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().gameObject.transform.name;
            CmdSetUsername(GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().gameObject.transform.name, _username);
        }

	}

    [Command]
    void CmdSetUsername(string _playerID, string _username)
    {
        PlayerManager player = GameManager.getPlayer(_playerID);
        if(player!=null)
        {
            Debug.Log(_username + " has joined");
            player.username = _username;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();
        GameManager.RegisterPlayer(_netID, _player);

    }

    void OnDisable()
    {
        Destroy(playerUIInstance);
        if(isLocalPlayer)
            GameManager.instance.setSceneCameraActive(true);
        GameManager.UnregisterPlayer(GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().gameObject.transform.name);
    }
	// Update is called once per frame
	void Update () {
        
    }
    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
}
