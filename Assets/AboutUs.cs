using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutUs : MonoBehaviour {

	public void ShowAbout()
    {
        if (gameObject.transform.Find("About"))
        {
            bool status = gameObject.transform.Find("About").gameObject.activeSelf;
            Debug.Log("found: "+!status);
            gameObject.transform.Find("About").gameObject.SetActive(!status);
        }
        else
            Debug.Log("Not found");
       
    }
    public void HideAbout()
    {
        if (gameObject.transform.Find("About"))
        {
            Debug.Log("found");
            gameObject.transform.Find("About").gameObject.SetActive(false);
        }
        else
            Debug.Log("Not found");

    }
}
