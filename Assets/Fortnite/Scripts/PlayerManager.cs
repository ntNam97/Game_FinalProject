using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PlayerSetup))]
public class PlayerManager : NetworkBehaviour
{
    public int kills;
    public int deaths;
    public bool inZone;
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get
        {
            return _isDead;
        }

        protected set
        {
            _isDead = value;
        }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    [SyncVar]
    private int currentHealth;
    public float getHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

    [SyncVar]
    public string username = "Loading...";

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjOnDeath;

    private bool[] wasEnabled;

    public GameObject _BRS_Mechanics;
    public BRS_PlayerHealthManager _PHM;
    private bool firstSetup = true;

    private PostProcessingBehaviour postProcessingBehaviour;
    [Header("---Post Processing Objects---")]
    public PostProcessingProfile outsideZonePPP;
    public PostProcessingProfile standardPPP;
    void Start ()
    {
        postProcessingBehaviour = GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().GetComponentInChildren<PostProcessingBehaviour>();
        
    }
    public void PlayerSetup()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.setSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        
        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClient();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClient()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefault();
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourcePlayerID)
    {
        if (isDead)
            return;
        currentHealth -= _amount;
        Debug.Log(GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name + "has have " + currentHealth + " health point");
        if(currentHealth<=0)
        {
            
            Die(_sourcePlayerID);
        }
    }
    private void Die(string _sourcePlayerID)
    {
        isDead = true;
        PlayerManager _sourcePlayer = GameManager.getPlayer(_sourcePlayerID);
        if(_sourcePlayer!=null)
        {
            _sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(username, _sourcePlayer.username);
           
        }
        
        deaths++;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjOnDeath.Length; i++)
        {
            disableGameObjOnDeath[i].SetActive(false);
        }
        
        GameObject _gfxIns=(GameObject) Instantiate(deathEffect, GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.position,Quaternion.identity);
        if(isLocalPlayer)
        {
            GameManager.instance.setSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
        Debug.Log(GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name + " is dead");
        Destroy(_gfxIns, 3f);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSetting.respawnTime);
        
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        PlayerSetup();
  
        Debug.Log("Player respawn");
    }
    private void SetDefault()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableGameObjOnDeath.Length; i++)
        {
            disableGameObjOnDeath[i].SetActive(true);
        }
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (PauseMenu.IsOn)
        {
           
            gameObject.transform.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled=false;
        }else
        {
            gameObject.transform.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        }
        if(!inZone)
        { postProcessingBehaviour.profile = outsideZonePPP; 
        } else
        {
            postProcessingBehaviour.profile = standardPPP;
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(10,transform.name);
        }
    }
}
