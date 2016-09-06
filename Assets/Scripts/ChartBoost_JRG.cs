using UnityEngine;
using UnityEngine.UI;
using ChartboostSDK;

public class ChartBoost_JRG : MonoBehaviour {
    public Text CBLogOutput;

    public void ChartBoostStart()
    {
        Chartboost.cacheInterstitial(CBLocation.GameScreen);
        CBLogOutput.text = "Attempting to cache Chartboost Ad.";

        if (Chartboost.hasInterstitial(CBLocation.GameScreen))
        {
            CBLogOutput.text = "Playing Chartboost Ad.";
            Chartboost.showInterstitial(CBLocation.GameScreen);
        }
        else { CBLogOutput.text = "No Chartboost video to show now. Try pressing Chartboost button again to request a video."; }
    }
}