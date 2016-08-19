using UnityEngine;
using AudienceNetwork;

public class FB_JRG : MonoBehaviour {
    public string placementId;
    public string deviceID;
    static public string FBStatusNotifier;

    private InterstitialAd interstitialAd;

    bool isLoaded;

    public void LoadFBInterstitial()
    {
        AdSettings.AddTestDevice("deviceID");
        InterstitialAd interstitialAd = new InterstitialAd(placementId);
        this.interstitialAd = interstitialAd;
        this.interstitialAd.Register(gameObject);

        this.interstitialAd.InterstitialAdDidLoad = (delegate ()
            {
               // Debug.Log("Interstitial Ad Loaded");
                FBStatusNotifier = "Interstitial Ad Loaded";        //Announce if the interstitial ad is loaded
                isLoaded = true;
            }
        );

        this.interstitialAd.InterstitialAdDidFailWithError = (delegate (string error)
        {
            Debug.Log("Interstitial Ad failed to load with error:" + error);
            FBStatusNotifier = "Interstitial Ad failed to load with error:" + error;
        });

        this.interstitialAd.LoadAd();
        PlayFBInterstitial();
    }

    public void PlayFBInterstitial()
    {
        if (isLoaded)
        {
           // Debug.Log("Showing interstial from : " + gameObject.name);
            FBStatusNotifier = "Showing interstial from : " + gameObject.name;
            interstitialAd.Show();
            //Debug.Log("Interstitial Ad is Playing");
        }else
        {
           // Debug.Log("Ad not loaded. Click Load Ad to request an ad");
            FBStatusNotifier = "Ad not loaded. Click Load Ad to request an ad";
        }
    }
}