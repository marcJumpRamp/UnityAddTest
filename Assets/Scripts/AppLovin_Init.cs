using UnityEngine;

public class AppLovin_Init : MonoBehaviour {
    
	void Start () {
        AppLovin.InitializeSdk();
        AppLovin.SetUnityAdListener("mobileAdServer_Canvas");
    }
}