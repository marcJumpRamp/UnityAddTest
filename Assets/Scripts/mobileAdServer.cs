﻿using UnityEngine;
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
        logOutput.text = AppLovin_JRG.ALStatusNotifier;
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
        logOutput.text = FB_JRG.FBStatusNotifier;
        Debug.Log(FB_JRG.FBStatusNotifier);
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
        logOutput.text = tapJoy_JRG.TJStatusNotifier;
        Debug.Log(tapJoy_JRG.TJStatusNotifier);
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