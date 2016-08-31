using UnityEngine;
using System.Collections;
using TapjoyUnity;

public class tapJoy_JRG : MonoBehaviour {
    static public string TJStatusNotifier;
    public string TapJoyPlacementName;
    public string Android_SDKKey;
    public string IOS_SDKKey;


    public void getTapJoyAd()
    {
        connectToTapJoy();
    }

    void connectToTapJoy()
    {
        if (!Tapjoy.IsConnected)
        {
            TJStatusNotifier = "Attempting to log into TapJoy...";
            Debug.Log("Attempting to log into TapJoy...");
            Tapjoy.Connect(Android_SDKKey);
            Tapjoy.OnConnectSuccess -= connectionMadeTapJoy;
        }
    }

    void connectionMadeTapJoy()
    {
        TJStatusNotifier = "Connection Successful. Preloading ad";
        Debug.Log("Connection Successful. Preloading ad");
        TJPlacement placeTJAd = TJPlacement.CreatePlacement(TapJoyPlacementName);       //Manage placements in TapJoy's Dashboard

        if(Tapjoy.IsConnected)
            placeTJAd.RequestContent();
        else {
            TJStatusNotifier = "Tapjoy SDK must be connected before you can request content.";
            Debug.Log("Tapjoy SDK must be connected before you can request content.");
        }

        if (placeTJAd.IsContentReady())
        {
            placeTJAd.ShowContent();
            Tapjoy.OnConnectSuccess += connectionMadeTapJoy;
        }
        else
        {
            TJStatusNotifier = "Tapjoy Video was not able to play.";
            Debug.Log("Tapjoy Video was not able to play.");
        }
    }
}