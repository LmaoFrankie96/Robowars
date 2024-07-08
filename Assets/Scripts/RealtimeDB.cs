using System.Collections;
using UnityEngine;
using Firebase.Database;
using System;

public class RealtimeDB : MonoBehaviour
{
    public FirebaseComm fComm;
    [SerializeField]
    public string userId = "user_unique_id";  // Use a unique identifier for the user
    DatabaseReference dbRef;
    private string userUniqueID;

    void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        userUniqueID = AuthManager.userId_str;
    }

    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        string key = null;

        GetJoystickAxes();
        // Check for WASD key inputs
        //if (Input.GetKey(KeyCode.A))
        //{
        //    rb.AddForce(Vector3.left);
        //    key = "A";
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    rb.AddForce(Vector3.right);
        //    key = "D";
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(Vector3.forward);
        //    key = "W";
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    rb.AddForce(Vector3.back);
        //    key = "S";
        //}
        if (Input.GetKey(KeyCode.Joystick1Button0))
        {
            key = "joystickbutton0";
        }

        if (key != null)
        {
            SendKeyInput(key);
        }
    }
    private void GetJoystickAxes()
    {

        //left joystick values
        float rightJoystickHorizontal = Input.GetAxis("RightJoystickHorizontal");
        float leftJoystickVertical = Input.GetAxis("LeftJoystickVertical");
        SendMovementInput(NormalizeJoystickVal(rightJoystickHorizontal), NormalizeJoystickVal(leftJoystickVertical));
        

    }
    private float NormalizeJoystickVal(float value)
    {

        return ((value + 1) / 2 * 255);

    }
   
    public void SaveDataFn()
    {
        string json = JsonUtility.ToJson(fComm);
        dbRef.Child(userUniqueID).Child(userId).SetRawJsonValueAsync(json);
    }

    public void LoadDataFn()
    {
        Debug.Log("Inside LoadDataFn");
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        var ServerData = dbRef.Child(userUniqueID).Child(userId).GetValueAsync();
        yield return new WaitUntil(predicate: () => ServerData.IsCompleted);

        if (ServerData.IsFaulted)
        {
            Debug.LogError("Error fetching data: " + ServerData.Exception);
            yield break;
        }

        if (ServerData.IsCompleted)
        {
            Debug.Log("Process is completed");
            DataSnapshot snapshot = ServerData.Result;
            string jsonData = snapshot.GetRawJsonValue();

            if (jsonData != null)
            {
                fComm = JsonUtility.FromJson<FirebaseComm>(jsonData);
                Debug.Log("Server data found: " + fComm.Key);
            }
            else
            {
                Debug.Log("No data found");
            }
        }
    }

    void SendKeyInput(string key)
    {
        Debug.Log("Inside SendKeyInput function");

        fComm = new FirebaseComm { Key = key, Name = "xyz" }; // Adjust the Name as needed
        string json = JsonUtility.ToJson(fComm);
        dbRef.Child(userUniqueID).Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            Debug.Log("json" + (json));
            if (task.IsCompleted)
            {
                Debug.Log("Key input sent successfully: " + key);
            }
            else
            {
                Debug.LogError("Failed to send key input: " + task.Exception);
            }
        });
        LoadDataFn();
    }
    void SendMovementInput(float movementHorizontal, float movementVertical)
    {

        Debug.Log("Inside SendSendMovementInput function");

        fComm = new FirebaseComm { MovementHorizontal = movementHorizontal, MovementVertical = movementVertical }; // Adjust the Name as needed
        string json = JsonUtility.ToJson(fComm);
        dbRef.Child(userUniqueID).Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            Debug.Log("json" + (json));
            if (task.IsCompleted)
            {
                Debug.Log("Mov input sent successfully: " + movementHorizontal + " " + movementVertical);
            }
            else
            {
                Debug.LogError("Failed to send key input: " + task.Exception);
            }
        });
        LoadDataFn();
    }
}

[Serializable]
public class FirebaseComm
{
    public string Key;
    public string Name;

    public int SpeedBoost;
    public float MovementHorizontal;
    public float MovementVertical;
    public int TeamScore;
}
