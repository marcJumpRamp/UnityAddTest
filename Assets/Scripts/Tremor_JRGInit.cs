using UnityEngine;

public class Tremor_JRGInit : MonoBehaviour {
    
	void Start () {
        TremorVideo.InitWithID("test");
        TremorVideo.sdkInitializedDelegate();
        TremorVideo.sdkInitializedDelegate = SdkInitialized;
        TremorVideo.adReadyDelegate = AdReady;
        TremorVideo.LoadAd();
    }

    public void SdkInitialized()
    {
        Debug.Log("TV entry point SdkInitialized");
    }

    public void AdReady(string success)
    {
        Debug.Log("TV entry point AdReady");
    }

}
