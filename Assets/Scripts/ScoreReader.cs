using System.IO;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class ScoreReader : MonoBehaviour
{
    // Assign your TextMeshPro UI elements from the Inspector
    public TextMeshProUGUI score1Text;
    public TextMeshProUGUI score2Text;
 

    // Path to your local JSON file
    private string jsonFilePath;

    // Define a class that matches the structure of your JSON file
    [System.Serializable]
    public class ScoreData
    {
        public int score1;
        public int score2;
    }

    private void Start()
    {
        // Set the path to your JSON file, you can use Application.dataPath or any local path
        jsonFilePath = "C:/scores.json"; // Example path
    }

    private void FixedUpdate()
    {
        if (SessionManager.Instance.TimerRunning() == true)
        {

            CheckScore();
        }

    }

    private void CheckScore()
    {

        if (File.Exists(jsonFilePath))
        {
            // Read the JSON file
            string jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into the ScoreData class
            ScoreData scores = JsonUtility.FromJson<ScoreData>(jsonData);

            // Map the data to TextMeshPro UI text objects
            score1Text.text = scores.score1.ToString();
            score2Text.text = scores.score2.ToString();
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + jsonFilePath);
        }
    }
}
