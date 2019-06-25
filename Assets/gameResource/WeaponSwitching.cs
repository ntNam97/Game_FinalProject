using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    private Animator WeaponAnim;
    private const float FREEZE = 10.0f;
    private const float EQUIP = 1.0f;
    private const float HOLSTER = 2.0f;

    private Animator PreAnim;

    // Use this for initialization
    void Start()
    {
        // Default all to not active
        //foreach (Transform weapon in transform)
        //{
          //weapon.gameObject.SetActive(false);
        //}

        // initialize weapon one
        selectedWeapon = 0;
        SelectWeapon(selectedWeapon, 0);

        StartCoroutine(AnimationWait(PreAnim));
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown("1"))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown("2") && (transform.childCount >= 2))
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown("3") && (transform.childCount >= 3))
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown("4") && (transform.childCount >= 4))
        {
            selectedWeapon = 3;
        }

        if (Input.GetKeyDown("5") && (transform.childCount >= 5))
        {
            selectedWeapon = 4;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
          SelectWeapon(selectedWeapon, previousSelectedWeapon);
        }

    }

    private void SelectWeapon(int current, int previous)
    {
        // the order of these is very important
        //Put the old gun away first
        Debug.Log(current.ToString());
        Debug.Log(previous.ToString());

        PreAnim = (transform.GetChild(previous).gameObject).GetComponent<Animator>();
        PreAnim.SetFloat("WeaponState", HOLSTER);

        AnimationWait(PreAnim);

        selectedWeapon = current;

        //Bring out the new gun
        (transform.GetChild(current).gameObject).SetActive(true);
        (transform.GetChild(current).gameObject).GetComponent<Animator>().SetFloat("WeaponState", EQUIP);


    }

    public IEnumerator AnimationWait(Animator PreAnim)
    {
        while (PreAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= PreAnim.GetCurrentAnimatorStateInfo(0).length)
        {
            yield return new WaitForSeconds(30);
        }
    }
}
