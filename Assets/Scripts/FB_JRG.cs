using UnityEngine;
using UnityEngine.UI;
using AudienceNetwork;

public class FB_JRG : MonoBehaviour {
    public string placementId = "267948490256774_269167170134906";
    public string deviceID = "80DA77B329456878";
    public Text FBLogOutput;

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
                Debug.Log("Interstitial Ad Loaded call back");
                FBLogOutput.text = "Interstitial Ad Loaded call back";        //Announce if the interstitial ad is loaded
                isLoaded = true;
                PlayFBInterstitial();
            }
        );

        this.interstitialAd.InterstitialAdDidFailWithError = (delegate (string error)
        {
            Debug.Log("Interstitial Ad failed to load with error:" + error);
            FBLogOutput.text = "Interstitial Ad failed to load with error:" + error;
        });
        
        this.interstitialAd.LoadAd();
        
    }

    public void PlayFBInterstitial()
    {
        if (isLoaded)
        {
            Debug.Log("Facebook Interstitial Ad is Playing");
            FBLogOutput.text = "Facebook Interstitial Ad is Playing";
            interstitialAd.Show();
        }
    }
}