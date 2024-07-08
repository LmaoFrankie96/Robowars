using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Firebase.Database;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public PhotonView playerPrefab0 , playerPrefab1;
    public GameObject team1, team2;
    public Canvas canvas;
    private string userUniqueID;
    private string playerID;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        //if (team1 != null)
        //{
        //    JoinTeam(0);
        //}q
        //else if(team2 != null) 
        //{
        //    JoinTeam(1);
        //}
    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void JoinTeam(int team)
    {
        //do we already have a team?
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {

            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
            {
                { "Team", team }
            };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }
      //  canvas.gameObject.SetActive(false);
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void OnJoinLobby()
    {
        Debug.Log("Lobby Joined");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a room and failed");
        //most likely no room
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }
    /*
     public override void OnJoinedRoom()
     {
        Debug.Log("joined a room- yay!");
        //maybe allocate a team here?

        if (PhotonNetwork.IsMasterClient)
        {
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log($"Team number {team} is being instantiated");
            //load the level- this syncs with all clients
            if (team == 0)
            {
                //get a spawn for the correct team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(0);
                PhotonNetwork.Instantiate(playerPrefab0.name, spawn.position, spawn.rotation);
            }
            else
            {
                //now for the red team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(1);
                PhotonNetwork.Instantiate(playerPrefab1.name, spawn.position, spawn.rotation);
            }
        }
    
    }
    */
}
