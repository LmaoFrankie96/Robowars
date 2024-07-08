using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Header("When building the game change the  test_noOfPlayersToJoin ")]
    [Header("to 1 and change the condition online no 120 and uncomment teh text ")] 
    [Header("also chang double equal to TO greater than or equal to")]
    public int test_noOfPlayersToJoin = 1;
    public int noOfPlayersToJoin = 4;

    public TMP_Text countText;
    public Button teamABtn;
    public Button teamBBtn;

    private int teamToJoin = -1;  // Store the team to join

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
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
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinTeam(int team)
    {
        teamToJoin = team;  // Set the team to join
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a room and failed");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)noOfPlayersToJoin });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room!");
        if (teamToJoin != -1)
        {
            AssignTeam(teamToJoin);
            teamToJoin = -1;  // Reset the teamToJoin after assigning
        }
        teamABtn.interactable = false;
        teamBBtn.interactable = false;
        UpdatePlayerCount();
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

        UpdateTeamCount(team);
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

        Debug.Log($"Updated team {team} count to {teamCount}");
        CheckTeamsFull(roomProps);
        Debug.Log("Call 2");
    }

    private void CheckTeamsFull(Hashtable roomProps)
    {
        int team0Count = roomProps.ContainsKey("Team0Count") ? (int)roomProps["Team0Count"] : 0;
        int team1Count = roomProps.ContainsKey("Team1Count") ? (int)roomProps["Team1Count"] : 0;

        Debug.Log($"Team 0 count is: {team0Count}");
        Debug.Log($"Team 1 count is: {team1Count}");

        //Change this when shipping to them 
        if (team0Count == test_noOfPlayersToJoin )//  && team1Count >= test_noOfPlayersToJoin)
        {
            Debug.Log("Both Teams ready. Loading gameplay!");
            PhotonNetwork.LoadLevel(2);
        }
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
    }
}
