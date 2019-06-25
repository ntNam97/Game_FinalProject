using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConroler : MonoBehaviour {
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Shoot");
        }
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("DoReload");
        }
    }
}
