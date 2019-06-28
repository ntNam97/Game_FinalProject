using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSetting matchSetting;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;
    void Awake()
    {
        if (instance!=null)
        {
            Debug.LogError("More than one GameManager in scene");

        }
        else { instance = this; }
        
    }
    #region Player registering
    // Use this for initialization
    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.GetComponentInChildren<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().transform.name = _playerID;
    }
    public static void UnregisterPlayer (string _playerID)
    {
        players.Remove(_playerID);

    }
    public static PlayerManager getPlayer(string _playerID)
    {
        return players[_playerID];
    }
    public void setSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;
        sceneCamera.SetActive(isActive);
    }
    public static PlayerManager[] getAllPlayer()
    {
        return players.Values.ToArray();
    }
    // Use this for initialization
    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();
    //    foreach (string _playerID in players.Keys)
    //    {
    //        Debug.Log("_playerID" + _playerID);
    //        GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
    //    }
    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}
    #endregion
}
