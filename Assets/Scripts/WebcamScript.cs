using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WebcamScript : MonoBehaviour
{
    private WebCamTexture webCamTex;
    private int currentCamIndex = 0;

    public RawImage display;
    //public TMP_Text displayText;

    private void Start()
    {
        StartStopCam_Clicked();
    }
    public void SwapCam_Clicked()
    {

        if (WebCamTexture.devices.Length > 0)
        {

            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;


            if (webCamTex != null)
            {
                StopCam();
               // displayText.text = "Start Camera";
                StartStopCam_Clicked();
            }
        }
    }

    public void StartStopCam_Clicked()
    {
        if (webCamTex != null)
        {
            StopCam();
           // displayText.text = "Start Camera";
        }

        else
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            webCamTex = new WebCamTexture(device.name);
            display.texture = webCamTex;
            webCamTex.Play();
           // displayText.text = "Stop Camera";
        }
    }
    private void StopCam()
    {
        display.texture = null;
        webCamTex.Stop();
        webCamTex = null;

    }
}
