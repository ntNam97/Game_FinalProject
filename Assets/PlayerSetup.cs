using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
    Camera sceneCamera;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            GetComponent<PlayerFire>().enabled = false;
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
            GameObject.Find("SceneCamera").SetActive(false);
            Debug.Log("isLocal");

        }
        
        registerPlayer();
        
	}

    private void registerPlayer()
    {
        string _ID = "Player:" + GetComponent<NetworkIdentity>().netId;
        GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().gameObject.transform.name = _ID;


    }

    void OnDisable()
    {

    }
	// Update is called once per frame
	void Update () {
        
    }
}
