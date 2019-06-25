using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {
    [SerializeField]
  
    Camera sceneCamera;
	// Use this for initialization
	void Start () {
		if(!isLocalPlayer)
        {
            Debug.Log("isNotLocal");
           
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
        
	}
	void OnDisable()
    {

    }
	// Update is called once per frame
	void Update () {
        
    }
}
