using UnityEngine;
using UnityEngine.UI;

public class AppLovin_JRG : MonoBehaviour {
    public Text ALLogOutput;

    public void loadAppLovinAd()
    {
        ALLogOutput.text = "Attempting to load Apps Lovin Video";
        Debug.Log("Attempting to load Apps Lovin Video");
        AppLovin.PreloadInterstitial();

        if (AppLovin.HasPreloadedInterstitial())
        {
            ALLogOutput.text = "Apps Lovin Video is playing";
            Debug.Log("Apps Lovin Video is playing");
            AppLovin.ShowInterstitial();
        }
        else { ALLogOutput.text = "Apps Lovin Video is not available"; Debug.Log("Apps Lovin Video is not available"); }
    }

    void onAppLovinEventReceived(string eventId)
    {
        Debug.Log(string.Format("onAppLovinEventReceived({0})", eventId));
        switch (eventId)
        {
            case "HIDDENINTER":
            case "HIDDENREWARDED":
                ALLogOutput.text = "Apps Lovin Video has been hidden";
                Debug.Log("Apps Lovin Video has been hidden");
                break;
            case "LOADINTERFAILED":
            case "LOADREWARDEDFAILED":
                ALLogOutput.text = "Apps Lovin Video failed to load";
                Debug.Log("Apps Lovin Video failed to load");
                break;
            case "REWARDAPPROVED":
                ALLogOutput.text = "Apps Lovin Rewarded Video has been approved";
                Debug.Log("Apps Lovin Rewarded Video has been approved");
                break;
        }
    }
}