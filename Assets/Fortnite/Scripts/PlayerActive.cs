using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find("LocalPlayer(Clone)"))
        {
            GetComponentInChildren<LineRenderer>().enabled = true;
            GetComponentInChildren<BRS_ChangeCircle>().enabled = true;
        }
	}
}
