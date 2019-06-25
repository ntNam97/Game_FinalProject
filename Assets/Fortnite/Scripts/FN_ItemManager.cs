using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//=============================================================================
// This script was created by Matt Parkin as a part of the Polygon Pilgrimage
// Go to www.youtube.com/mattparkin and subscribe for more awesome tutorials!
// You are free to use/modify/edit this script however you like.  Please give
// credit to the orginal creator and share your work!
//=============================================================================
public class FN_ItemManager : MonoBehaviour
{
    [Header("--Setup Parameters---")]
    public GameObject ToolTipWidget;
    //public RawImage ItemBackground;
    public InventoryItem ItemParameters;

    //Private variables
    private TextMeshProUGUI[] TMPTexts;
    private TextMeshProUGUI PickUpButton;
    private TextMeshProUGUI TMP_ItemType;
    private TextMeshProUGUI TMP_ItemName;
    private TextMeshProUGUI TMP_ItemRarity;
    private TextMeshProUGUI TMP_ItemAmount;

    private bool isAdded = false;

    //NEW=================================================================
    private GameObject _player;
    private bool inRange = false;
    private GameObject ItemHolder;
    private Animator anim;
    private GameObject _BRS_Mechanics;
    private BRS_InventoryManager _IM;
    //NEW=================================================================

    // Use this for initialization
    void Start()
    {
        //NEW=================================================================
        //Get a handle to the BRS_Mechanics and the Inventory Manager script
        _BRS_Mechanics = GameObject.Find("BRS_Mechanics");
        _IM = _BRS_Mechanics.GetComponent<BRS_InventoryManager>();

        //Get a handle to the ItemHolder object
        var IHTrans = this.gameObject.transform.GetChild(1);
        ItemHolder = IHTrans.gameObject;

        //Get the ItemHolder's Animator
        anim = gameObject.GetComponent<Animator>();
        //NEW=================================================================

        //Get all of the Text Mesh Pros
        TMPTexts = ItemParameters.LootPickUp.GetComponentsInChildren<TextMeshProUGUI>();
        //TMPTexts = gameObject.GetComponentsInChildren<TextMeshProUGUI> ();

        //Go through all the Texts and based upon what it is, set values
        for (int i = 0; i < TMPTexts.Length; i++)
        {
            switch (TMPTexts[i].name)
            {
                case "_PickUpBtnText":
                    TMPTexts[i].text = ItemParameters.PickUpButtonText;
                    //PickUpButton = TMPTexts [i];
                    //PickUpButton.text = PickUpButtonText;

                    //NEW=================================================================
                    //Convert our PickUp button text to lowercase
                    //PickUpButtonText = PickUpButtonText.ToLower();
                    break;

                case "_txtType":
                    TMPTexts[i].text = ItemParameters.ItemType;
                    break;

                case "_txtItemName":
                    TMPTexts[i].text = ItemParameters.ItemName;
                    break;

                case "_txtRarity":
                    TMPTexts[i].text = ItemParameters.ItemRarity.ToString();
                    break;

                case "_txtAmount":
                    PickUpButton = TMPTexts[i];
                    //PickUpButton.text = ItemAmmount;
                    break;
            }
        }
        //Based upon what Rarity we choose, set the background color
        switch (ItemParameters.ItemRarity.ToString())
        //switch (TMP_ItemRarity.text)
        {
            case "Common":
                //ItemBackground.color = Common;
                break;

            case "Uncommon":
                //ItemBackground.color = Uncommon;
                break;

            case "Rare":
                //ItemBackground.color = Rare;
                break;

            case "Epic":
                //ItemBackground.color = Epic;
                break;

            case "Legendary":
                //ItemBackground.color = Legendary;
                break;

            case "Mythic":
                //ItemBackground.color = Mythic;
                break;
        }

        //Make the widget "invisible" (deactivate) for now
        ToolTipWidget.SetActive(false);
    }

    //When we enter the trigger...
    void OnTriggerEnter(Collider col)
    {
        //If the game object that triggered us is the Player...
        if (col.transform.tag == "Player")
        {
            //Make the widget visible (active)
            ToolTipWidget.SetActive(true);

            //Save the identity of the Player
            _player = col.gameObject;

            //Set our In Range variable to true, we are in range
            inRange = true;
        }
    }

    //When we exit the trigger...
    void OnTriggerExit(Collider col)
    {
        //If the game object that triggered us is the Player...
        if (col.transform.tag == "Player")
        {
            //Make the widget invisible (deactivate)
            ToolTipWidget.SetActive(false);

            //Set our In Range variable to false, we are not in range
            inRange = false;
        }
    }
    //NEW=================================================================
    void Update()
    {
        //If we are in range and we press the correct button
        if (Input.GetKeyDown(ItemParameters.PickUpButtonText) && inRange)
        {
            //Try to add this item to our inventory
            if ((_IM.AddToInventory(ItemParameters)) != null)
            {
                //Set the item as a child of the Player
                this.transform.parent = _player.transform;

                //Move the item to the base of the player
                this.transform.localPosition = Vector3.zero;

                //Play the gathering animation
                anim.SetTrigger("addToInv");

                //Destory the object, now that we have captured it
                Destroy(gameObject, 1.0f);
            }
        }
    }
}