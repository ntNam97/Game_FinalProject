using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePoke : MonoBehaviour {
    public GameObject character;
    public GameObject[] characters;
    public GameObject information;
    public GameObject []informations;
    public GameObject button;

    int curIndex=12;
	// Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void nextPoke()
    {
        Destroy(character);
        Destroy(information);
        curIndex++;
        button.GetComponent<ChangePoke>().curIndex = curIndex;
        character = Instantiate(characters[curIndex%12], character.transform.parent);
        information = Instantiate(informations[curIndex % 12], information.transform.parent);
        button.GetComponent<ChangePoke>().character = character;
        button.GetComponent<ChangePoke>().information = information;

    }
    public void prevPoke()
    {
        
        Destroy(character);
        Destroy(information);
        curIndex--;
        button.GetComponent<ChangePoke>().curIndex = curIndex;
        character = Instantiate(characters[curIndex%12], character.transform.parent);
        information = Instantiate(informations[curIndex % 12], information.transform.parent);
        button.GetComponent<ChangePoke>().character = character;
        button.GetComponent<ChangePoke>().information = information;
    }
}
