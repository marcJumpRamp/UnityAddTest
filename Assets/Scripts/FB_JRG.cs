using UnityEngine;
using AudienceNetwork;

public class FB_JRG : MonoBehaviour {
    public string placementId = "267948490256774_269167170134906";
    public string deviceID = "80DA77B329456878";
    static public string FBStatusNotifier;

    private InterstitialAd interstitialAd;

    bool isLoaded;

    public void LoadFBInterstitial()
    {
        Debug.Log("Load FB Interstitial");
        AdSettings.AddTestDevice(deviceID);
        InterstitialAd interstitialAd = new InterstitialAd(placementId);
        this.interstitialAd = interstitialAd;
        this.interstitialAd.Register(gameObject);

        this.interstitialAd.InterstitialAdDidLoad = (delegate ()
            {
                Debug.Log("Interstitial Ad Loaded");
                FBStatusNotifier = "Interstitial Ad Loaded";        //Announce if the interstitial ad is loaded
                isLoaded = true;
            }
        );

        this.interstitialAd.InterstitialAdDidFailWithError = (delegate (string error)
        {
            Debug.Log("Interstitial Ad failed to load with error:" + error);
            FBStatusNotifier = "Interstitial Ad failed to load with error:" + error;
            Debug.Log("Ad not loaded. Click Facebook to request an ad");
            FBStatusNotifier = "Ad not loaded. Click Facebook to request an ad";
        });
        
        this.interstitialAd.LoadAd();

        if(isLoaded)
            PlayFBInterstitial();
    }

    void PlayFBInterstitial()
    {
        Debug.Log("Facebook Interstitial Ad is Playing");
        FBStatusNotifier = "Facebook Interstitial Ad is Playing";
        interstitialAd.Show();
    }
}