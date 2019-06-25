using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=============================================================================
// This script was created by Matt Parkin as a part of the Polygon Pilgrimage
// Go to www.youtube.com/mattparkin and subscribe for more awesome tutorials!
// You are free to use/modify/edit this script however you like.  Please give
// credit to the orginal creator and share your work!
//=============================================================================
public class BRS_InventoryManager : MonoBehaviour
{
    public int numSlots = 6;
    public InventoryItem[] ItemInv;
    public int nextFreeSlot;
    public GameObject newIcon;
    public GameObject InventoryUIArea;
    private Transform[] SlotCenters;

    private GameObject SlotFrame;

    public FN_ItemManager inItemMgr;
    public string inItemName;
    public bool canStack = false;
    public bool canAdd = false;

    private bool isViewingInventory = true;
    public GameObject InventoryOverlay;
    private Animator ThirdPersonAnimator;
    public GameObject TPC;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController PlayerControlScript;

    private GameObject _BRS_Mechanics;
    //private BRS_UIManager _UIM;

    // Use this for initialization
    void Start()
    {
        InventoryOverlay = GameObject.Find("ViewingInventory");
        InventoryOverlay.SetActive(false);
        TPC = GameObject.FindGameObjectWithTag("Player");
        ItemInv = new InventoryItem[numSlots];
        _BRS_Mechanics = GameObject.Find("BRS_Mechanics");
        //_UIM = GameObject.Find ("BRS_UI").GetComponent<BRS_UIManager> ();
        InventoryUIArea = GameObject.Find("InvUI");
        //ThirdPersonAnimator = TPC.GetComponent<Animator>();
        PlayerControlScript = TPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        SlotCenters = InventoryUIArea.GetComponentsInChildren<Transform>();

        //foreach(Transform t in SlotCenters)
        //{
        //Debug.Log(t.position.y);
        //}

        //Get the Slot Frame
        SlotFrame = GameObject.Find("SlotFrame");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("i"))
        {
            isViewingInventory = InvScreenViewing(isViewingInventory);
        }

        //We ignore the 0 index because for some reason a list of its children includes itself

        if (Input.GetKeyUp("1"))
        {
            Debug.Log("slotFrame" + SlotFrame.name);
            SlotFrame.transform.position = SlotCenters[1].position;
            UpdateIcon(1);
        }

        if (Input.GetKeyUp("2"))
        {
            SlotFrame.transform.position = SlotCenters[2].position;
            UpdateIcon(2);
        }

        if (Input.GetKeyUp("3"))
        {
            SlotFrame.transform.position = SlotCenters[3].position;
            UpdateIcon(3);
        }

        if (Input.GetKeyUp("4"))
        {
            SlotFrame.transform.position = SlotCenters[4].position;
            UpdateIcon(4);
        }

        if (Input.GetKeyUp("5"))
        {
            SlotFrame.transform.position = SlotCenters[5].position;
            UpdateIcon(5);
        }

        if (Input.GetKeyUp("6"))
        {
            SlotFrame.transform.position = SlotCenters[6].position;
            UpdateIcon(6);
        }
    }

    public void UpdateIcon(int Slot)
    {
        GameObject[] ListofTiles = GameObject.FindGameObjectsWithTag("UIDraggable");
        foreach (GameObject go in ListofTiles)
        {
            //Set them all to unselcted first
            var SelectedIcon = go.transform.GetChild(0);
            var UnSelectedIcon = go.transform.GetChild(1);

            SelectedIcon.gameObject.SetActive(false);
            UnSelectedIcon.gameObject.SetActive(true);

            //Then find the one that matches our x value and set it to selected
            if (go.transform.position.x == SlotCenters[Slot].position.x)
            {
                SelectedIcon.gameObject.SetActive(true);
                UnSelectedIcon.gameObject.SetActive(false);
            }
        }
    }

    public bool InvScreenViewing(bool viewState)
    {
        if (viewState == true)
        {
            //If we are going to show the inventory
            //ThirdPersonAnimator.SetFloat("Forward", 0f);
            //ThirdPersonAnimator.SetFloat("Turn", 0f);
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

    public InventoryItem AddToInventory(InventoryItem newItem)
    {
        canAdd = false;

        //Get a look at what type of item we are adding
        //inItemMgr = newItem.GetComponent<FN_ItemManager> ();

        //Get the name of the item
        inItemName = newItem.ItemName;
        //Debug.Log (inItemName);

        //Do we already have one of those items?
        //TODO - Full Paid Asset coming soon!

        //Is it stackable?
        canStack = newItem.isStackable;

        //If we don't already have one, and it is stackable...
        if (canStack)
        {
            //Debug.Log ("Increase the inventory for this item.");
            return newItem;
        }
        else
        {
            //If we don't already have one, and its NOT stackable...
            //Debug.Log("Attempt to add it to our inventory if we have room");
        }

        //Do we have room?
        //If we get back a -1 that means that we are full up!
        if (NextFreeSlot() < 0)
        {
            //Debug.Log ("You are overincumbered!");
            return null;
        }
        else
        {
            //Debug.Log ("added to inventory");
            var newSlotID = NextFreeSlot();

            ItemInv[newSlotID] = newItem;
            canAdd = true;

            //_UIM.AddInventoryIcon (newItem, newSlotID);
            AddInventoryIcon(newItem, newSlotID);

            //Future development - what to do now that we have this item??
        }
        //canAdd = false;
        return newItem;
    }

    public void RemoveFromInventory(int SlotNum)
    {
        var dropItem = ItemInv[SlotNum-1].LootPickUp;
        ItemInv[SlotNum-1] = null;
        Instantiate(dropItem, TPC.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void UpdateInventory(int SlotNum1, int SlotNum2)
    {
        var tempSlot = ItemInv[SlotNum2];
        ItemInv[SlotNum2] = ItemInv[SlotNum1];
        ItemInv[SlotNum1] = tempSlot;
    }

    public int NextFreeSlot()
    {
        nextFreeSlot = System.Array.IndexOf(ItemInv, null);
        //Return the next free slot index
        return nextFreeSlot;
    }

    public void AddInventoryIcon(InventoryItem item, int SlotNum)
    {
        newIcon = Instantiate(item.InvTile);
        newIcon.transform.SetParent(InventoryUIArea.transform);
        newIcon.transform.localScale = new Vector3(1, 1, 1);

        var Magic = 40 + (SlotNum * 92);
        newIcon.transform.localPosition = new Vector3((Magic), 40, 0);
    }
}