using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthBarFill;
    private PlayerManager player;

    UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
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
        
    }

    private void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
