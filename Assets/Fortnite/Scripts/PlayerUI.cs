using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthBarFill;
    [SerializeField]
    GameObject pauseMenu;
    private PlayerManager player;

    UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    void Start()
    {
        PauseMenu.IsOn = false;
    }
    public void setPlayer(PlayerManager _player)
    {
        player = _player;
        controller = player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }
    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void Update()
    {
        SetFuelAmount(controller.getThrusterFuelAmount());
        SetHealthAmount(player.getHealthPct());
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        
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
