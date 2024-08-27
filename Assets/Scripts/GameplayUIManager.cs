using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager _instance;
    public GameObject[] uIScreens;
    public GameObject databaseManager;
    public GameObject nightVisionUI;
    public Slider loadingBar;
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

    void Start()
    {
        databaseManager.SetActive(false);
    }

    //Functions to change the login screen

    /*  public void ShowSplashScreen()
      {
          OpenUIScreen(2);
          splashManager.SetActive(true);
      }*/
    public void ShowLoadingScreen()
    {
        OpenUIScreen(1);
        StartCoroutine(FillSlider());
        ShowMenuScreen();

    }
    public void ShowMenuScreen()
    {
        //StopCoroutine(FillSlider());
        OpenUIScreen(2);
    }
    public void ShowMatchTypeScreen()
    {
        Debug.Log("Showing MatchType Screen");
        OpenUIScreen(3);
    }
    /*public void ShowAvatarSelectionScreen()
    {
        OpenUIScreen(4);
    }*/
    public void ShowGameplayScreen()
    {
        
        databaseManager.SetActive(true);
        OpenUIScreen(3);
    }
    private void OnDisable()
    {
        //OpenUIScreen(0);
    }
    public void ToggleNightVision() {

        if (nightVisionUI != null) {

            nightVisionUI.SetActive(!nightVisionUI.activeSelf);
        }
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


    }
}
