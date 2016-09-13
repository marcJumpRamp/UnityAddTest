using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tremor_JRG : MonoBehaviour {
    public Text TLogOutput;

  /*  void Start()
    {
        //TremorVideo.sdkInitializedDelegate = SdkInitialized;
        TremorVideo.adReadyDelegate = TremorAdReady;
        TremorVideo.adCompleteDelegate = TremorAdDone;
    }

    public void SdkInitialized()
    {
        Debug.Log("SdkInitialized");
    }
    public void TremorAdStart()
    {
        TLogOutput.text = "Tremor is attempting to load an ad.";
        TremorVideo.LoadAd();
    }
    */
    public void TremorAdPlay()
    {
        TLogOutput.text = "Tremor is showing an ad.";
        TremorVideo.ShowAd();
    }

    /*
    void TremorAdDone(string complete)
    {
        Debug.Log("Tremor finished playing an ad.");
    }
    */
}