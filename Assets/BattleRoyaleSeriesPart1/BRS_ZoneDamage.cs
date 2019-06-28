using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PostProcessing;

public class BRS_ZoneDamage : MonoBehaviour
{
	[Header("---Player Object---")]
	//private GameObject gameManagerHolder;
   // private GameManager gameManager;
	private PlayerManager[] players;

	[Header("---Post Processing Objects---")]
	public PostProcessingProfile outsideZonePPP;
	public PostProcessingProfile standardPPP;
	private PostProcessingBehaviour CamPPB;

	[Header("---Zone Damage Parameters---")]
	public float TickRate = 3.0f;
	public int TickDamage = 1;

	//Future values
	//For each succesive zone change we will damage
	//the player more for being outside the zone
	//public int zoneDamageMultiplier;


	//Are we in the Zone?
	private bool inZone;

	//For DEMO purposes ONLY.  Reset our Health to full when we
	//re-enter the Zone!
	private bool DebugHealth = false;

	void Start ()
	{
        //Get a handle to the Player
        //gameManager = gameManagerHolder.GetComponent<GameManager>();
        players = GameManager.getAllPlayer();
        
		//CamPPB = player.GetComponentInChildren<PostProcessingBehaviour> ();
        //playername = player.transform.name;

        //Get a handle to the Player Health Manager
		//Setup the DamagePlayer to run every X seconds
		InvokeRepeating ("DamagePlayer", 0.0f, TickRate);
	}
    void Update()
    {
        players = GameManager.getAllPlayer();
    }

	void OnTriggerExit(Collider col)
	{
        //If we leave the zone, we will be damaged!
        foreach (PlayerManager player in players)
        {
            if (col.transform.name == player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name)
            {
                Debug.Log(player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name+ "out");
                //CamPPB.profile = outsideZonePPP;
                player.inZone = false;
            }
        }
		
	}

	void OnTriggerEnter(Collider col)
	{
        //If we are inside the zone, all is good!
        foreach (PlayerManager player in players)
        {
            if (col.transform.name == player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name)
            {
                Debug.Log(player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name+" in");
                //CamPPB.profile = standardPPP;
                player.inZone = true;
            }
        }
        
	}
    
	void DamagePlayer()
	{
        foreach (PlayerManager player in players)
        {
            if (!player.inZone)
            {
                //Damage the player [TickDamage] amount
                player.RpcTakeDamage(TickDamage, transform.name);
                Debug.Log(player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name + " out of zone -" + TickDamage + " health point");
                //_PHM.ChangeHealth (-TickDamage);
            }
            else if (DebugHealth)
            {
                
                //_PHM.ChangeHealth (100);
            }
        }
        
	}
}