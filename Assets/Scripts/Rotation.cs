using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    // Use this for initialization
    public GameObject character;
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void rotateLeft()
    {
        character.transform.Rotate(Vector3.up * 500.0f * Time.deltaTime);
    }
    public void rotateRight()
    {
        character.transform.Rotate(Vector3.up * (-500.0f) * Time.deltaTime);
    }
}
