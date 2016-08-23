using UnityEngine;
using System.Collections;

public class AppLovin_JRG : MonoBehaviour {
    static public string ALStatusNotifier;

    public void loadAppLovinAd()
    {
        AppLovin.PreloadInterstitial();

        if (AppLovin.HasPreloadedInterstitial())
        {
            ALStatusNotifier = "Apps Lovin Video is playing";
            AppLovin.ShowInterstitial();
        }
    }

    void onAppLovinEventReceived(string eventId)
    {
        Debug.Log(string.Format("onAppLovinEventReceived({0})", eventId));
        switch (eventId)
        {
            case "HIDDENINTER":
            case "HIDDENREWARDED":
                ALStatusNotifier = "Apps Lovin Video has been hidden";
                break;
            case "LOADINTERFAILED":
            case "LOADREWARDEDFAILED":
                ALStatusNotifier = "Apps Lovin Video failed to load";
                break;
            case "REWARDAPPROVED":
                ALStatusNotifier = "Apps Lovin Rewarded Video has been approved";
                break;
        }
    }
}