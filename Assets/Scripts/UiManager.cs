using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager _instance;
    public GameObject loginScreen;
    public GameObject registerScreen;
    private bool isPasswordVisible = false;
    //public GameObject[] uIScreens;
    //public GameObject splashManager;

    // public Slider loadingBar;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Debug.Log("Instance already exists");
            Destroy(this);
        }
    }

    //Functions to change the login screen

    public void ShowLoginScreen()
    {
        loginScreen.SetActive(true);
        registerScreen.SetActive(false);
        //OpenUIScreen(0);

    }
    public void ShowRegisterScreen()
    {
        loginScreen.SetActive(false);
        registerScreen.SetActive(true);
        // OpenUIScreen(1);
    }
    public void TogglePasswordVisibility(TMP_InputField inputField)
    {
        if (isPasswordVisible)
        {
            // Change to password field
            inputField.contentType = TMP_InputField.ContentType.Password;
            isPasswordVisible = false;
        }
        else
        {
            // Change to standard field
            inputField.contentType = TMP_InputField.ContentType.Standard;
            isPasswordVisible = true;
        }

        // Manually update the text to trigger the change
        inputField.ForceLabelUpdate();
    }
    public void QuitGame()
    {
        // If we are running in the editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If we are running in a build, quit the application
        Application.Quit();
#endif
    }

    /*   public void ShowDataScreen()
       {
           OpenUIScreen(8);
       }
       public void ShowSplashScreen()
       {
           OpenUIScreen(2);
           splashManager.SetActive(true);
       }
       public void ShowLoadingScreen()
       {
           OpenUIScreen(3);
           StartCoroutine(FillSlider());
           ShowMenuScreen();

       }
       public void ShowMenuScreen()
       {
           //StopCoroutine(FillSlider());
           OpenUIScreen(4);
       }
       public void ShowCapSelectionScreen()
       {
           OpenUIScreen(5);
       }
       public void ShowAvatarSelectionScreen()
       {
           OpenUIScreen(6);
       }
       public void ShowGameplayScreen()
       {
           OpenUIScreen(7);
       }
       private void OnDisable()
       {
           OpenUIScreen(0);
       }

       private void OpenUIScreen(int screenNumber)
       {
           for (int i = 0; i < uIScreens.Length; i++)
           {
               if (i == screenNumber)
               {
                   uIScreens[i].SetActive(true);
               }
               else
               {
                   uIScreens[i].SetActive(false);
               }
           }
       }
       IEnumerator FillSlider()
       {
           float duration = 5f; // 2 seconds
           float startTime = Time.realtimeSinceStartup;

           while (Time.realtimeSinceStartup < startTime + duration)
           {
               float elapsed = Time.realtimeSinceStartup - startTime;
               loadingBar.value = Mathf.Clamp01(elapsed / duration);
               yield return null;
           }

           // Ensure the slider is full at the end
           loadingBar.value = 1f;


       }*/
}
