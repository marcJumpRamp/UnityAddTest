using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TVEntryPoint : MonoBehaviour {
    public Text TVLogOutput;


    void Start () {
        TremorVideo.InitWithID("test");
        TremorVideo.sdkInitializedDelegate = SdkInitialized;
        TremorVideo.adReadyDelegate = AdReady;
        TremorVideo.adCompleteDelegate = AdDone;
    }
	
    public void SdkInitialized()
    {
        Debug.Log("TV entry point SdkInitialized");
    }

    public void LoadAd()
    {
        TVLogOutput.text = "Loading Tremor Video";
        Debug.Log("Tremor is attempting to load video");
        TremorVideo.LoadAd();
    }

    public void AdReady(string success)
    {
        TVLogOutput.text = "Tremor Video Ready";
        Debug.Log(success);

        ShowAd();
    }

    public void ShowAd()
    {
        TremorVideo.ShowAd();
        TVLogOutput.text = "Tremor Video Playing";
        Debug.Log("Tremor is attempting to show video");
    }

    public void AdDone(string videoComplete)
    {
        TVLogOutput.text = "Tremor Video Complete";
        Debug.Log(videoComplete);
    }
}