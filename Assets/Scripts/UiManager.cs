using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager _instance;
    public GameObject[] uIScreens;
    public GameObject splashManager;

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
        OpenUIScreen(0);

    }
    public void ShowRegisterScreen()
    {
        OpenUIScreen(1);
    }
    public void ShowDataScreen()
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
    }
    public void ShowMenuScreen()
    {
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
}
