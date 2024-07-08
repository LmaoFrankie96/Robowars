using UnityEngine;

public class SplashManager : MonoBehaviour
{
    
    private void Update()
    {
        if (GameplayUIManager._instance.uIScreens[0] != null)
        {
            if (GameplayUIManager._instance.uIScreens[0].activeInHierarchy)
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
            Debug.Log("I am pressed");
            GameplayUIManager._instance.ShowLoadingScreen();
            this.gameObject.SetActive(false);
        }
    }
}
