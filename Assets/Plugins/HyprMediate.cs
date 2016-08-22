using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// HYPRMEDIATE
// This class is your point of contact with the HyprMediate SDK.
// Use its public static methods to use HyprMediate's features from anywhere in a scene.
// This script should be a component of exactly one GameObject in your scene. That GameObject will persist
// as the game moves from scene to scene, so do not add it to any other scenes.
// The SDK comes with a HyprMediate prefab that can act as the persistent GameObject.

public class HyprMediate : MonoBehaviour
{
	// LIFE CYCLE METHODS
	// This method's use of DontDestroyOnLoad() keeps the HyprMediate GameObject persisting across scenes.
	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}

	// IMPORT IOS FUNCTIONS
	// These functions allow the Unity engine to send messages to native iOS code.
	// They're wrapped in a check for whether or not the app is running on a real iOS device.

	#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern void _initializeHyprMediate (string apiToken, string userID, string unityInfo);

	[DllImport ("__Internal")]
	private static extern void _checkInventory ();

	[DllImport ("__Internal")]
	private static extern void _showAd ();

	[DllImport ("__Internal")]
	private static extern void _setUserID (string userID);

	[DllImport ("__Internal")]
	private static extern void _setLogLevel (int logLevel);

	[DllImport ("__Internal")]
	private static extern int _logLevel ();

	[DllImport ("__Internal")]
	private static extern int _hyprMediateSDKVersionNumber ();
	#endif

	// CALLBACK DELEGATE
	static HyprMediateListener _listener;

	// PUBLIC PROPERTIES
	public static HyprMediateLogLevel logLevel {
		get { return GetLogLevel (); }
	}

	public static int sdkVersionNumber {
		get { return GetSDKVersionNumber (); }
	}

	public static HyprMediateListener listener {
		get { return _listener; }
	}

	// PUBLIC METHODS
	// These public, static methods allow Unity code anywhere in the app to execute HyprMediate commands.

	// Initialize the HyprMediate SDK. Call this method before making any attempt to check or show ads.
	public static void Initialize (string apiToken, string userID, HyprMediateListener listener)
	{
		HyprMediate._listener = listener;
		HyprMediateUnityInfo unityInfo = new HyprMediateUnityInfo ();
		unityInfo.unityVersion = Application.unityVersion;
		string infoString = JsonUtility.ToJson (unityInfo);
#if UNITY_IOS
		_initializeHyprMediate (apiToken, userID, infoString);
#elif UNITY_ANDROID
		AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject interpreter = new AndroidJavaObject("com.hyprmx.mediate.UnityInterpreter");
		interpreter.CallStatic("initialize", context, apiToken, userID, infoString);
#endif
	}

	// Check the availability of ads - availability information is returned via the inventory delegate.
	public static void CheckInventory ()
	{
#if UNITY_IOS
		_checkInventory ();
#elif UNITY_ANDROID
		AndroidJavaObject interpreter = new AndroidJavaObject("com.hyprmx.mediate.UnityInterpreter");
		interpreter.CallStatic("checkInventory");
#endif
	}

	// Show an ad - information about the ad's display, reward and possible error is delievered by delegates.
	public static void ShowAd ()
	{
#if UNITY_IOS
		_showAd ();
#elif UNITY_ANDROID
		AndroidJavaObject interpreter = new AndroidJavaObject("com.hyprmx.mediate.UnityInterpreter");
		interpreter.CallStatic("showAd");
#endif
	}

	// Set the user id that's included with each analytics event.
	public static void SetUserID (string userID)
	{
#if UNITY_IOS
		_setUserID (userID);
#elif UNITY_ANDROID
		GetHyprMediateInstance().Call("setUserId", userID);
#endif
	}

	// Set the log level for logs from the iOS SDK.
	public static void SetLogLevel (HyprMediateLogLevel logLevel)
	{
#if UNITY_IOS
		_setLogLevel ((int)logLevel);
#endif
	}

	// Set the listener for messages from HyprMediate
	public static void SetListener (HyprMediateListener listener) 
	{
		HyprMediate._listener = listener;
	}

	// PRIVATE METHODS
	// These private methods back HyprMediate's public properties and methods.
	// DO NOT call these methods yourself.

	private static HyprMediateLogLevel GetLogLevel ()
	{
		int level = 0;
#if UNITY_IOS
		level = _logLevel ();
#endif

		return (HyprMediateLogLevel)level;
	}

	private static int GetSDKVersionNumber ()
	{
		int versionNumber = 0;
#if UNITY_IOS
		versionNumber = _hyprMediateSDKVersionNumber ();
#elif UNITY_ANDROID
		versionNumber = GetHyprMediateInstance().Call<int>("version");
#endif

		return versionNumber;
	}

	private static HyprMediateReward RewardFromString (string rewardString)
	{
		HyprMediateReward reward = JsonUtility.FromJson<HyprMediateReward> (rewardString);
		return reward;
	}

	private static HyprMediateError ErrorFromString (string errorString)
	{
		HyprMediateError error = JsonUtility.FromJson<HyprMediateError> (errorString);
		return error;
	}

#if UNITY_ANDROID
	private static AndroidJavaObject GetHyprMediateInstance() {
		return new AndroidJavaClass("com.hyprmx.mediate.HyprMediate").CallStatic<AndroidJavaObject>("getInstance");
	}
#endif

	// CALLBACK METHODS
	// These are methods that get called by native iOS code to execute a callback.
	// DO NOT call these methods yourself.

	void NativeInventoryCallback (string callbackString)
	{
		if (HyprMediate.listener != null) {
			bool canShowAd = (callbackString == "true");
			HyprMediate.listener.HyprMediateCanShowAd (canShowAd);
		}
	}

	void NativeRewardCallback (string callbackString)
	{
		if (HyprMediate.listener != null) {
			HyprMediateReward result = RewardFromString (callbackString);
			HyprMediate.listener.HyprMediateRewardDelivered (result);
		}
	}

	void NativeErrorCallback (string callbackString)
	{
		if (HyprMediate.listener != null) {
			HyprMediateError error = ErrorFromString (callbackString);
			HyprMediate.listener.HyprMediateErrorOccurred (error);
		}
	}

	void NativeAdStartedCallback (string callbackString)
	{
		if (HyprMediate.listener != null) {
			HyprMediate.listener.HyprMediateAdStarted ();
		}
	}

	void NativeAdFinishedCallback (string callbackString)
	{
		if (HyprMediate.listener != null) {
			HyprMediate.listener.HyprMediateAdFinished ();
		}
	}

	// PRIVATE HELPER CLASSES
	// This class helps record Unity-specific build information in HyprMediate's analytics.

	[System.Serializable]
	class HyprMediateUnityInfo : System.Object
	{
		public string unityVersion;
	}
}



// PUBLIC HELPER CLASSES AND ENUMS
// These public classes and enum give object structure to information about HyprMediate activity.

// HyprMediateReward is a class representing the result of showing an ad.
[System.Serializable]
public class HyprMediateReward : System.Object
{
	public int virtualCurrencyAmount;
	public string virtualCurrencyName;
}

// HyprMediateError is a class representing an error arising from attempted ad display.
[System.Serializable]
public class HyprMediateError : System.Object
{
	public string errorType;
	public string errorTitle;
	public string errorDescription;
}

// HyprMediateLogLevel is an enum representing the level at which the iOS SDK is outputting logs.
public enum HyprMediateLogLevel
{
	HyprMediateLogLevelError,
	HyprMediateLogLevelVerbose,
	HyprMediateLogLevelDebug
}

public interface HyprMediateListener
{
	void HyprMediateCanShowAd (bool canShowAd);
	void HyprMediateRewardDelivered (HyprMediateReward result);
	void HyprMediateErrorOccurred (HyprMediateError error);
	void HyprMediateAdStarted ();
	void HyprMediateAdFinished ();
}