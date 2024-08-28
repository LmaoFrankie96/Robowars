using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public int test_noOfPlayersToJoin = 1;
    private int test_totalnoOfPlayers;
    public int noOfPlayersToJoin = 4;
    private int total_noofPlayersToJoin;

    public TMP_Text countText;
    public Button teamABtn;
    public Button teamBBtn;
    public GameObject loadingScreen;

    private int team0Count = 0;
    private int team1Count = 0;

    //public GameObject realtimeDB;
    private int teamToJoin = -1;  // Store the team to join
    private Button[] teamBtns = new Button[2];
    private bool teamSelected = false;


    public float timeLeft = 100f; // 5 minutes, for example
    public TMP_Text timerText;
    private bool isConnecting;

    PhotonView photonView;

    public GameObject playerPrefab;
    //  we need to put loading screen on the time when PHOTON PUN IS CONNECTING TO SERVER
    //For it We need to understand where it is doing that then put loading screen there

    private void Awake()
    {
        teamBtns[0] = teamABtn;
        teamBtns[1] = teamBBtn;

        //  photonView = GetComponent<PhotonView>();

        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("INside Awake method");

    }

    void Start()
    {
        loadingScreen.SetActive(true);
        test_totalnoOfPlayers = test_noOfPlayersToJoin + test_noOfPlayersToJoin; // for testing 
        Debug.Log("test_totalnoOfPlayers " + test_totalnoOfPlayers);
        total_noofPlayersToJoin = noOfPlayersToJoin + noOfPlayersToJoin;

        for (int i = 0; i < 2; i++)
        {
            teamBtns[i].gameObject.SetActive(false);
        }

        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    // Request the current time from the master client when a non-master client joins or starts.
        //    photonView.RPC("RequestTimeSync", RpcTarget.MasterClient);
        //}
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Inisde Connect method");
        isConnecting = true;
    }

    public override void OnConnectedToMaster()
    {
        //   base.OnConnectedToMaster();
        Debug.Log(" On Connected To Master ");
        #region
        // Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);

        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        //if (isConnecting)
        //{
        //    Debug.Log("OnConnectedToMaster: Next -> try to Join Random Room");
        //    Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

        //    for (int i = 0; i < 2; i++)
        //    {
        //        teamBtns[i].gameObject.SetActive(true);
        //    }
        //    loadingScreen.SetActive(false);
        //}
        #endregion

        Debug.Log("OnConnectedToMaster: Next -> try to Join Random Room");
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

        for (int i = 0; i < 2; i++)
        {
            teamBtns[i].gameObject.SetActive(true);
            teamBtns[i].interactable = true;
        }
        loadingScreen.SetActive(false);

    }

    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //void Update()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        if (timeLeft > 0)
    //        {
    //            timeLeft -= Time.deltaTime;
    //            UpdateTimerDisplay();
    //        }
    //        else
    //        {
    //            timeLeft = 0;
    //            PhotonNetwork.LeaveRoom();
    //        }
    //    }
    //}

    //void UpdateTimerDisplay()
    //{
    //    timerText.text = "Time Left: " + Mathf.FloorToInt(timeLeft).ToString();
    //    Debug.Log("timerText" + timeLeft);
    //   // photonView.RPC("SyncTime", RpcTarget.Others, timeLeft);
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        photonView.RPC("SyncTime", RpcTarget.Others, timeLeft);
    //    }
    //}

    [PunRPC]
    //void SyncTime(float syncedTime)
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        timeLeft = syncedTime;
    //        UpdateTimerDisplay();
    //    }
    //}

    //[PunRPC]
    //void RequestTimeSync()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        photonView.RPC("SyncTime", RpcTarget.Others, timeLeft);
    //    }
    //}


    public void JoinTeam(int team)
    {
        if (!teamSelected)
        {
            teamToJoin = team;  // Set the team to join
            PhotonNetwork.JoinRandomRoom();
            teamSelected = true;
            SetTeam(team);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a room and failed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)noOfPlayersToJoin });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room!");

        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        Debug.Log("Player Prefab instantiated " + playerPrefab.name);
        if (teamToJoin != -1)
        {
            AssignTeam(teamToJoin);
            teamToJoin = -1;  // Reset the teamToJoin after assigning
        }
        teamABtn.interactable = false;
        teamBBtn.interactable = false;
        UpdatePlayerCount();

        if (PhotonNetwork.CurrentRoom.PlayerCount == test_totalnoOfPlayers || PhotonNetwork.CurrentRoom.PlayerCount == total_noofPlayersToJoin)
        {
            //  StartGameTimer();
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            Debug.Log("Player Prefab instantiated " + playerPrefab.name);
        }
    }

    private void AssignTeam(int team)
    {
        // Check if the player already has a team
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            Hashtable playerProps = new Hashtable
            {
                { "Team", team }
            };
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }
        // From here the team value need to go to realtime DB 

        // Generate a unique player ID
        string playerId = PhotonNetwork.LocalPlayer.UserId;

        // Update the team count
        UpdateTeamCount(team);

        // Send the team and player ID to the RealtimeDB
      //  if (RealtimeDB.instance != null)
        //{
        //    //    RealtimeDB.instance.UpdateRealtimeDB(team, playerId);
        //}
    }

    private void UpdateTeamCount(int team)
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("No room available to update team count.");
            return;
        }

        Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        string teamPropKey = "Team" + team + "Count";

        int teamCount = roomProps.ContainsKey(teamPropKey) ? (int)roomProps[teamPropKey] : 0;
        teamCount++;
        roomProps[teamPropKey] = teamCount;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

        if (team == 0)
        {
            team0Count = teamCount;
        }

        else if (team == 1)
        {
            team1Count = teamCount;
        }

        // Send the updated team count to the RealtimeDB
        //if (RealtimeDB.instance != null)
        //{
        //    //  RealtimeDB.instance.UpdateTeamCountInDB(team, teamCount);
        //}

        Debug.Log($"Updated team {team} count to {teamCount}");
        CheckTeamsFull(roomProps);
        Debug.Log("Call 2");
    }

    private void CheckTeamsFull(Hashtable roomProps)
    {
        team0Count = roomProps.ContainsKey("Team0Count") ? (int)roomProps["Team0Count"] : 0;
        team1Count = roomProps.ContainsKey("Team1Count") ? (int)roomProps["Team1Count"] : 0;

        Debug.Log($"Team 0 count is: {team0Count}");
        Debug.Log($"Team 1 count is: {team1Count}");

        if (team0Count == test_noOfPlayersToJoin && team1Count == test_noOfPlayersToJoin)
        {
            Debug.Log("Both Teams ready. Loading gameplay!");
            PhotonNetwork.LoadLevel(2);
        }
        else if (team0Count > test_noOfPlayersToJoin && team1Count > test_noOfPlayersToJoin)
        {
            Debug.Log("Player count exceeds!");
            countText.text = "Player Count Exceeded! Wait for room to be available";
            return;
        }
    }
    private void SetTeam(int teamNumber)
    {

        PlayerPrefs.SetInt("Team", teamNumber);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Team"))
        {
            Debug.Log($"Player {targetPlayer.NickName} changed team to {changedProps["Team"]}");
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        CheckTeamsFull(propertiesThatChanged);
        Debug.Log("Call 1");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} joined the room.");
        UpdatePlayerCount();

        if (PhotonNetwork.CurrentRoom.PlayerCount == test_totalnoOfPlayers || PhotonNetwork.CurrentRoom.PlayerCount == total_noofPlayersToJoin)
        {
            //  StartGameTimer();
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left the room.");

        UpdatePlayerCount();
        UpdateTeamCountAfterPlayerLeft();
    }

    private void UpdatePlayerCount()
    {
        int totalPlayers = PhotonNetwork.CountOfPlayers;
        countText.text = $"Waiting for players to join. {totalPlayers}/8";
    }

    private void UpdateTeamCountAfterPlayerLeft()
    {
        Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        int team0Count = 0;
        int team1Count = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
                if (team == 0)
                {
                    team0Count++;
                }
                else if (team == 1)
                {
                    team1Count++;
                }
            }
        }

        roomProps["Team0Count"] = team0Count;
        roomProps["Team1Count"] = team1Count;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

        // Update the team count in the database
        //if (RealtimeDB.instance != null)
        //{
        //    //RealtimeDB.instance.UpdateTeamCountInDB(0, team0Count);
        //    //RealtimeDB.instance.UpdateTeamCountInDB(1, team1Count);
        //}
    }
    //private void StartGameTimer()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        timeLeft = 300.0f; // Reset or set the timer
    //        photonView.RPC("SyncTime", RpcTarget.Others, timeLeft);
    //        UpdateTimerDisplay();
    //    }
    //}
}
