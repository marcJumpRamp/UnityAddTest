using UnityEngine;
using System.Collections;

public class hyprListener : MonoBehaviour, HyprMediateListener {
	// Prevent this GameObject from re-initializing
	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
	}

	void Start ()
	{
		StartHyprMediate ();
	}

	public void StartHyprMediate ()
	{
		string apiToken = "";
		#if UNITY_IOS
		apiToken = "87ed7e18-56eb-4972-a2b3-2bf0e9b26970"; // API token for your iOS app
		#elif UNITY_ANDROID
		//apiToken = "4cbe0ae1-be3a-4815-bd9e-50dc87901568"; // Live Android App
		apiToken = "b76e0c5e-d919-4763-ba54-deda5257ff28"; // Test Android App
		#endif
		string userId = "800873486";
		#if !UNITY_EDITOR
		// Don't initialize in the Unity Editor, as trying to call HyprMediate from there will not work.
		HyprMediate.Initialize (apiToken, userId, this);
		#endif
	}

	public void HyprMediateCanShowAd (bool canShowAd)
	{
		if (canShowAd) {
			HyprMediate.ShowAd ();
		} else {
			Debug.Log ("No Fill");
		}
	}

	public void HyprMediateAdStarted ()
	{
		// Stop any music or animations in your application since the ad is now playing
	}

	public void HyprMediateAdFinished ()
	{
		// Restart any music or animations in your application now that the ad is finished.
	}

	public void HyprMediateRewardDelivered (HyprMediateReward reward)
	{
		Debug.Log ("Ad complete. Reward earned. Reward: " + reward.virtualCurrencyAmount + " " + reward.virtualCurrencyName);
		// Give the player their reward
		//Set Semaphore here
	}

	public void HyprMediateErrorOccurred (HyprMediateError error)
	{
		Debug.Log ("HyprMediate error:\n" + error.errorType + "\n" + error.errorTitle + "\n" + error.errorDescription);
	}

	public void HandleButton(){
		HyprMediate.CheckInventory ();
	}
}