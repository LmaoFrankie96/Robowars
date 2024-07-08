using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{

  //  public GameObject redPlayerPrefab;
   // public GameObject bluePlayerPrefab;
   // public GameObject pauseCanvas;
    //public bool paused = false;


    private void Start()
    {
        
    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {

            GameplayUIManager._instance.ToggleNightVision();
        }
    }

    /*void SetPaused()
    {
        //set the canvas
        pauseCanvas.SetActive(paused);
        //set the cursoro lock
       // Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        //set the cursoro visible
      //  Cursor.visible = paused;
    }*/

    //reload the level when anyone leaves or joins?- That is done in the demo but is it needed?
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //(int)PhotonNetwork.LocalPlayer.CustomProperties["TeamCount"]=;
        ReloadLevel();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Reloading Level");
            PhotonNetwork.LoadLevel(2);
        }
    }


}
