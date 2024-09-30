using System.Collections;
using UnityEngine;
using TMPro;
public class SessionManager : MonoBehaviour
{
    // Singleton instance
    public static SessionManager Instance { get; private set; }
    // Duration of the timer in seconds
    public int timerDuration = 5; // Set this to the number of seconds you want (e.g., 5 seconds)

    // Internal variable to track elapsed time
    private int elapsedTime = 0;
    private int roundCount = 3;
    // Boolean to check if the timer is running
    private bool timerRunning = false;

    public GameObject roundStatusObj;
    public GameObject settingsUi;
    public GameObject roundFinishUi;
    public TMP_InputField timerInputField;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI roundStatusText;
    public TextMeshProUGUI nextRoundTimeText;
    private Animator animator;


    private int nextTimerDuration = 5;
    private int nextTimerElapsed = 0;

    // Ensures that only one instance of the SessionManager exists
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the instance across scenes
        }
    }

    private void Start()
    {
        if (animator == null) {

            animator = roundStatusObj.GetComponent<Animator>();
        }
    }

    // Coroutine to run the timer every second    
    private IEnumerator RunTimer()
    {
        for (int i = 1; i <= roundCount; i++)
        {
            timerRunning = true;

            while (elapsedTime < timerDuration)
            {
                yield return new WaitForSeconds(1f); // Wait for 1 second
                elapsedTime++;
                Debug.Log("Elapsed Time: " + elapsedTime);

                // Do something when the time increments, if needed
                timerText.text = (timerDuration - elapsedTime).ToString();
            }
            //next round sequence
            
            
            yield return StartCoroutine(EndRoundSequence());

            

            // Timer has finished
            Debug.Log("Timer Finished!");
            timerRunning = false;
            if (i <= roundCount)
            {
                elapsedTime = 0;
            }
        }
        StopGame();
    }

    // Function to start the timer
    public void StartTimer()
    {
        if (!timerRunning)
        {
            if (int.TryParse(timerInputField.text, out int userInputDuration))
            {
                timerDuration = userInputDuration; // Set the timerDuration to the user input value
                elapsedTime = 0; // Reset the elapsed time
                StartCoroutine(RunTimer()); // Start the coroutine
            }
        }
    }

    // Function to stop the timer if needed
    public void StopGame()
    {
        StopCoroutine(RunTimer());
        //timerRunning = false;
        settingsUi.SetActive(true);
    }

    public bool TimerRunning()
    {

        return timerRunning;
    }
    public void CloseGame() {

        Application.Quit();
    }

    private IEnumerator WaitForAnimationToEnd(string stateName)
    {
        roundStatusObj.SetActive(true);
        Debug.Log("Inside Animation coroutine");
        // Play the animation
        animator.Play(stateName);

        // Wait for the animation to start playing
        yield return new WaitForEndOfFrame();

        // Get the Animator's current state information
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animation finishes playing
        while (stateInfo.normalizedTime < 1.0f)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        // Do something after the animation finishes
        Debug.Log("Animation Finished!");
        roundStatusObj.SetActive(false);
        nextRoundTimeText.enabled = true;
        
    }

    private IEnumerator NextRoundTimer() {

        while (nextTimerElapsed < nextTimerDuration)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            nextTimerElapsed++;
            //Debug.Log("Elapsed Time: " + elapsedTime);

            // Do something when the time increments, if needed
            nextRoundTimeText.text = "NEXT ROUND IN \n"+(nextTimerDuration - nextTimerElapsed).ToString();
        }
        nextTimerElapsed = 0;
        nextRoundTimeText.enabled = false;
        StopCoroutine(NextRoundTimer());
    }
    private IEnumerator EndRoundSequence() {

        
        roundStatusText.enabled = true;
        roundFinishUi.SetActive(true);

        yield return StartCoroutine(WaitForAnimationToEnd("RoundStatus"));

        yield return StartCoroutine(NextRoundTimer());

        roundFinishUi.SetActive(false);
    }
}
