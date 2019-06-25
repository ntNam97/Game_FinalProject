using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorM4 : MonoBehaviour {
    public Animator anim;
    
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            
            anim.SetBool("bFire", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("bFire", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            anim.SetBool("bScoped", !anim.GetBool("bScoped"));
        }
        bool moved = false;
        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.S))
        {
            moved = true;
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && moved  == true)
        {
            Debug.Log("true");
            anim.SetBool("bRun", true);
            anim.SetBool("bWalk", false);
        }
        else if(moved)
        {
            Debug.Log("false");
            anim.SetBool("bWalk", true);
            anim.SetBool("bRun", false);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            moved = false;
            anim.SetBool("bWalk", false);
            anim.SetBool("bRun", false);
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("bRun", false);
        }
        //if (Input.GetMouseButtonUp(1))
        //{
        //    anim.SetBool("bScoped", false);
        //}
    }
}
