using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem")]
public class InventoryItem : ScriptableObject
{
    //Public variables
    [Header("---Pick Up Item Tool Tip Parameters---")]
    public string ItemName;
    public string ItemType;
    [Header("---Inventory UI Tile---")]
    public GameObject InvTile;

    [Header("---Loot PickUp---")]
    public GameObject LootPickUp;

    public enum ItemRarityEnum
    {
        Common, Uncommon, Rare, Epic, Legendary, Mythic
    }
    public ItemRarityEnum ItemRarity;
    public Color Common;
    public Color Uncommon;
    public Color Rare;
    public Color Epic;
    public Color Legendary;
    public Color Mythic;
    public string ItemAmmount;
    public bool isStackable = false;
    public string PickUpButtonText;
}
