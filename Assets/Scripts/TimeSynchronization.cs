using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TimeSynchronization : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_Text countdownText; // Assign a UI Text component in the Inspector
    public double countdownDuration = 300; // 5 minutes in seconds
    private double startTime;
    private bool countdownStarted = false;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Master client sets the start time and sends it to all clients
            startTime = PhotonNetwork.Time;
            photonView.RPC("StartCountdown", RpcTarget.AllBuffered, startTime);
        }
    }

    [PunRPC]
    void StartCountdown(double networkStartTime)
    {
        startTime = networkStartTime;
        countdownStarted = true;
    }

    void Update()
    {
        if (countdownStarted)
        {
            double elapsedTime = PhotonNetwork.Time - startTime;
            double remainingTime = countdownDuration - elapsedTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                countdownStarted = false;
                // Handle countdown completion (e.g., end game, show message, etc.)
            }

            DisplayTime(remainingTime);
        }
    }

    void DisplayTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60);
        int seconds = Mathf.FloorToInt((float)time % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
