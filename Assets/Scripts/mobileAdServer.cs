using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mobileAdServer : MonoBehaviour {
    public Text logOutput;

    public void WaterFallAds()
    {
        logOutput.text = "Activating Waterfall";
        Debug.Log("Activating Waterfall");
    }

    public void AdColonyAds()
    {
        logOutput.text = "Activating Ad Colony Ads";
        Debug.Log("Activating Ad Colony Ads");
    }

    public void AppLovinAds()
    {
        logOutput.text = "Activating App Lovin Ads";
        Debug.Log("Activating App Lovin Ads");
    }

    public void AdMarvelAds()
    {
        logOutput.text = "Activating Ad Marvel Ads";
        Debug.Log("Activating Ad Marvel Ads");
    }

    public void ChartBoostAds()
    {
        logOutput.text = "Activating Chart Boost Ads";
        Debug.Log("Activating Chart Boost Ads");
    }

    public void FacebookAds()
    {
        logOutput.text = "Activating Facebook Ads";
        Debug.Log("Activating Facebook Ads");
    }

    public void HyprMxAds()
    {
        logOutput.text = "Activating HyprMx Ads";
        Debug.Log("Activating HyprMx Ads");
    }

    public void LiveRailAds()
    {
        logOutput.text = "Activating LiveRail Ads";
        Debug.Log("Activating LiveRail Ads");
    }

    public void MediaBrixAds()
    {
        logOutput.text = "Activating Media Brix Ads";
        Debug.Log("Activating Media Brix Ads");
    }

    public void PretioAds()
    {
        logOutput.text = "Activating Pretio Ads";
        Debug.Log("Activating Pretio Ads");
    }

    public void TapJoyAds()
    {
        logOutput.text = "Activating Tap Joy Ads";
        Debug.Log("Activating Tap Joy Ads");
    }

    public void TremorAds()
    {
        logOutput.text = "Activating Tremor Ads";
        Debug.Log("Activating Tremor Ads");
    }

    public void VungleAds()
    {
        logOutput.text = "Activating Vungle Ads";
        Debug.Log("Activating Vungle Ads");
    }

    public void QuitAp()
    {
        logOutput.text = "Good Bye :)";
        Debug.Log("Good Bye :)");
        Application.Quit();
    }
}