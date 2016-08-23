using UnityEngine;
using System.Collections;

public class AppLovin_JRG : MonoBehaviour {
    static public string ALStatusNotifier;

    public void loadAppLovinAd()
    {
        ALStatusNotifier = "Attempting to load Apps Lovin Video";
        Debug.Log("Attempting to load Apps Lovin Video");
        AppLovin.PreloadInterstitial();

        if (AppLovin.HasPreloadedInterstitial())
        {
            ALStatusNotifier = "Apps Lovin Video is playing";
            Debug.Log("Apps Lovin Video is playing");
            AppLovin.ShowInterstitial();
        }
        else { ALStatusNotifier = "Apps Lovin Video is not available"; Debug.Log("Apps Lovin Video is not available"); }
    }

    void onAppLovinEventReceived(string eventId)
    {
        Debug.Log(string.Format("onAppLovinEventReceived({0})", eventId));
        switch (eventId)
        {
            case "HIDDENINTER":
            case "HIDDENREWARDED":
                ALStatusNotifier = "Apps Lovin Video has been hidden";
                Debug.Log("Apps Lovin Video has been hidden");
                break;
            case "LOADINTERFAILED":
            case "LOADREWARDEDFAILED":
                ALStatusNotifier = "Apps Lovin Video failed to load";
                Debug.Log("Apps Lovin Video failed to load");
                break;
            case "REWARDAPPROVED":
                ALStatusNotifier = "Apps Lovin Rewarded Video has been approved";
                Debug.Log("Apps Lovin Rewarded Video has been approved");
                break;
        }
    }
}