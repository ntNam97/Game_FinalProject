using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    RectTransform healthBarFill;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreBoard;
    private PlayerManager player;
    private WeaponManager weaponManager;
    UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    void Start()
    {
        PauseMenu.IsOn = false;
    }
    public void setPlayer(PlayerManager _player)
    {
        player = _player;
        controller = player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        weaponManager = _player.GetComponent<WeaponManager>();
    }
    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void Update()
    {
        SetFuelAmount(controller.getThrusterFuelAmount());
        SetHealthAmount(player.getHealthPct());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }

    }

    private void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }
    private void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
