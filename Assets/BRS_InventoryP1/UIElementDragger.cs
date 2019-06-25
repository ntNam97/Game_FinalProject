using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Copyright (C) Xenfinity LLC - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Bilal Itani <bilalitani1@gmail.com>, June 2017
 */

public class UIElementDragger : MonoBehaviour
{

    public const string DRAGGABLE_TAG = "UIDraggable";
    public const string STATIC_TAG = "UIStatic";  // it is imperative that ONLY the InvSlot GameObjects have this tag

    private float[,] UIBoundaries = new float[,]
    {
      //left, right
      {800f, 1130f},
      //bottom, top
      {35f, 90f}
    };
    private bool dragging = false;

    private Vector2 originalPosition;
    private Transform objectToDrag;
    private Image objectToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();
    private BRS_InventoryManager _IM;
    private GameObject[] UISlots;  //UI InventorySlots
    private int[] _slots = new int[2];

    #region Monobehaviour API

    void Start()
    {
        _IM = GameObject.Find("BRS_Mechanics").GetComponent<BRS_InventoryManager>();
        UISlots = GameObject.FindGameObjectsWithTag(STATIC_TAG);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objectToDrag = GetDraggableTransformUnderMouse();

            if (objectToDrag != null)
            {
                //BRS - Addition
                // remember the slot number
                _slots[0] = GetUISlotNumber(objectToDrag.position);

                dragging = true;

                objectToDrag.SetAsLastSibling();

                originalPosition = objectToDrag.position;
                objectToDragImage = objectToDrag.GetComponent<Image>();
                objectToDragImage.raycastTarget = false;
            }
        }

        if (dragging)
        {
            objectToDrag.position = Input.mousePosition;
            //Debug.Log(objectToDrag.position);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (objectToDrag != null)
            {
                var objectToReplace = GetDraggableTransformUnderMouse();

                if (BoundaryCheck(objectToDrag.position))
                {
                    //BRS - Addition
                    //Debug.Log("OBJECT OUT OF RANGE");
                    Destroy(objectToDrag.gameObject);
                    _IM.RemoveFromInventory(_slots[0]); ClearUISlots();
                    //Debug.Log("INSTANTIATE A NEW PICKUP ITEM HERE");
                    objectToDrag = null;
                    dragging = false;
                    return;
                }
                //BRS - Addition
                // We need the ability to drop an item on an empty slot
                // it also must snap in place
                if (objectToReplace != null)
                {
                    _slots[1] = GetUISlotNumber(objectToReplace.position);

                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.position = originalPosition;
                }
                else
                {
                    objectToDrag.position = GetStaticTransformUnderMouse().position;
                    _slots[1] = GetUISlotNumber(objectToDrag.position);
                }

                _IM.UpdateInventory(_slots[0], _slots[1]); ClearUISlots();

                objectToDragImage.raycastTarget = true;
                objectToDrag = null;
            }

            dragging = false;
        }
    }

    //BRS - Addition
    // Reports if the object position exceeds the defined boundaries
    private bool BoundaryCheck(Vector3 pos)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;  //probably will not use this

        if ((x < UIBoundaries[0, 0] || x > UIBoundaries[0, 1]) || (y < UIBoundaries[1, 0] || y > UIBoundaries[1, 1]))
            return true;

        // implied 'else' because we fell-through
        return false;
    }

    // This is a convenience method - reduces the repetitive code
    private void ClearUISlots()
    {
        _slots[0] = _slots[1] = -1;
    }

    private int GetUISlotNumber(Vector3 forPosition)
    {
        foreach (GameObject UISlot in UISlots)
        {
            if (UISlot.transform.position.x == forPosition.x)
            {
                int lastChar = UISlot.name.Length - 1;
                return System.Int32.Parse(UISlot.name[lastChar].ToString());
            }
        }
        return -1;
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        return hitObjects.First().gameObject;
    }

    private Transform GetDraggableTransformUnderMouse()
    {
        var clickedObject = GetObjectUnderMouse();

        // get top level object hit
        if (clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }

        return null;
    }

    private Transform GetStaticTransformUnderMouse()
    {
        var clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && clickedObject.tag == STATIC_TAG)
        {
            return clickedObject.transform;
        }
        return null;
    }

    #endregion
}
