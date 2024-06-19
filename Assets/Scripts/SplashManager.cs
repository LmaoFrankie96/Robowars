using UnityEngine;

public class SplashManager : MonoBehaviour
{
    
    private void Update()
    {
        if (UiManager._instance.uIScreens[2] != null)
        {
            if (UiManager._instance.uIScreens[2].activeInHierarchy)
            {
                PressAnyKey();
            }
        }
        else
        {
            Debug.Log("Splash is empty");
        }
    }
    private void PressAnyKey()
    {
        if (Input.anyKeyDown)
        {
            UiManager._instance.ShowLoadingScreen();
            this.gameObject.SetActive(false);
        }
    }
}
