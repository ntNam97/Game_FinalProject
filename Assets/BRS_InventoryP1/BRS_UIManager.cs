using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRS_UIManager : MonoBehaviour
{
    //public GameObject EjectPrompt;
    ///private GameObject DropZone;
    //private BRS_DropZoneBehavior DZB;
    //private BRS_PlaneManager PlaneManager;

    public GameObject InventoryIconBase;
    public GameObject InventoryUIArea;
    public GameObject newIcon;

    public RectTransform AreaTrans;
    private Transform[] SlotCenters;

    private bool isViewingInventory = true;
    public GameObject InventoryOverlay;
    private Animator ThirdPersonAnimator;
    public GameObject TPC;
    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl PlayerControlScript;

    // Use this for initialization
    void Start()
    {
        AreaTrans = InventoryUIArea.GetComponent<RectTransform>();
        //DropZone = GameObject.Find ("AcceptableDropZone");
        //DZB = DropZone.GetComponent<BRS_DropZoneBehavior> ();
        ThirdPersonAnimator = TPC.GetComponent<Animator>();
        PlayerControlScript = TPC.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>();

        SlotCenters = InventoryUIArea.GetComponentsInChildren<Transform>();


    }

    // Update is called once per frame
    void Update()
    {
        //if (DZB.canJump)
        //{
        //	EjectPrompt.SetActive (true);
        //
        //	if (Input.GetKeyUp ("f"))
        //	{
        //		GameObject AirPlane = GameObject.Find ("BRS_Plane(Clone)");
        //		BRS_PlaneManager PlaneMgr = AirPlane.GetComponent<BRS_PlaneManager> ();
        //		PlaneMgr.SpawnPlayer (AirPlane.transform.position);
        //		EjectPrompt.SetActive (false);
        //		DZB.playerJumped = true;
        //	}

        if (Input.GetKeyUp("i"))
        {
            isViewingInventory = InvScreenViewing(isViewingInventory);
        }

        if (Input.GetKeyUp("p"))
        {
            AddInventoryIcon(0);

        }
        //}
    }

    public bool InvScreenViewing(bool viewState)
    {
        if (viewState == true)
        {
            //If we are going to show the inventory
            ThirdPersonAnimator.SetFloat("Forward", 0f);
            ThirdPersonAnimator.SetFloat("Turn", 0f);
            PlayerControlScript.enabled = false;

            //Show the overlay
            InventoryOverlay.SetActive(true);
        }
        else
        {
            //If we are NOT going to show the inventory
            PlayerControlScript.enabled = true;

            //Hide the overlay
            InventoryOverlay.SetActive(false);
        }
        return !viewState;
    }

    public void AddInventoryIcon(int SlotNum)
    {
        //Debug.Log ("adding inventory icon");
        newIcon = Instantiate(InventoryIconBase);
        newIcon.transform.SetParent(InventoryUIArea.transform);
        newIcon.transform.localScale = new Vector3(1, 1, 1);

        var Magic = 40 + (SlotNum * 92);
        newIcon.transform.localPosition = new Vector3((Magic), 40, 0);
    }
}
