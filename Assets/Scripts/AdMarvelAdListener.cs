using UnityEngine;
using System.Collections;
using System;
using AdMarvelAndroidAds;
using System.Collections.Generic;

public class AdMarvelAdListener : MonoBehaviour {
	AdMarvelInterstitial AdMarvelInterstitial;
	static bool AdMarvelInitialized;
	string partnerId = "c3f1482901cb9af6";
	string siteId = "148662";
	readonly Dictionary<string, string> TargetParams = new Dictionary<string, string> { { "targetParamKey", "targetParamValue" } };

	// Use this for initialization
	void Start () {
		
	}
	


	public void AdMarvelClick(){
		#if UNITY_ANDROID
		if (!AdMarvelInitialized)
		{
			var publisherIds = new Dictionary<string, string> { { AdMarvelSDKNetworks.ADMARVEL, "FILLLATER" } };
			AdMarvelUtils.initialize(publisherIds);

			AdMarvelInitialized = true;
		}
		#endif

		var dictionary = new Dictionary<string, string> { { "UserID", "P-CHARID" } };

		AdMarvelInterstitial = new AdMarvelInterstitial();
		#if UNITY_ANDROID
		AdMarvelInterstitial.AdMarvelInterstitialAdReceived += HandleInterstitialLoaded;
		AdMarvelInterstitial.AdMarvelInterstitialAdFailedToReceived += HandleInterstitialFailedToLoad;
		AdMarvelInterstitial.AdMarvelInterstitialAdClosed += HandleAdClosedEvent;
		#else
		AdMarvelInterstitial.GetInterstitialAdSucceeded += HandleInterstitialLoaded;
		AdMarvelInterstitial.GetInterstitialAdFailed += HandleInterstitialFailedToLoad;
		AdMarvelInterstitial.InterstitialClosed += HandleAdClosedEvent;
		AdMarvelInterstitial.SetTargettingParams(dictionary);
		#endif

		if (true)
		{
			#if UNITY_ANDROID
			AdMarvelInterstitial.AdMarvelRewardAdResult += HandleDidReceiveReward;
			AdMarvelInterstitial.requestRewardInterstitial(TargetParams,partnerId, siteId, dictionary);
			#else
			AdMarvelInterstitial.DidReceiveReward += HandleDidReceiveReward;
			AdMarvelInterstitial.GetRewardInterstitialAd(Model.PartnerId, Model.SiteId, BusinessLogic.Profile.UserId, null);
			#endif
		}
		else
		{
			#if UNITY_ANDROID
			AdMarvelInterstitial.requestNewInterstitialAd(TargetParams,partnerId, siteId);
			#else
			AdMarvelInterstitial.GetInterstitialAd(Model.PartnerId, Model.SiteId);
			#endif
		}
	}


	//todo: Hacky solution for AdMarvel SDK thread problem
	void Update()
	{
		if (AdClosed)
		{
			
			Debug.Log ("Ad Finished");
			AdClosed = false;

		}

		if (ReceivedReward)
		{
//			print("ReceivedReward in ViewUpdating: ReceivedRewardIsSuccess = " + ReceivedRewardIsSuccess);
//
//			ReceivedReward = false;
//			if (Model.Reward)
//			{
//				if (ReceivedRewardIsSuccess) 
//				{
//					BusinessLogic.NextOpportunityStep();
//					BusinessLogic.LastAdViewed = "AdMarvel";
//				}
//				else BusinessLogic.ShowLastMenuView();
//			}
			AdClosed = false;
			RecievedReward = false;
		}

		if (FailedToLoad)
		{
//			print("FailedToLoad in ViewUpdating: Model.Reward = " + Model.Reward);

			FailedToLoad = false;
//			Router.FailLabel();
		}
	}

	bool ReceivedReward;
	bool ReceivedRewardIsSuccess;

	#if UNITY_ANDROID
	void HandleDidReceiveReward(object sender, AdMarvelRewardAdResultEvent e)
	#else
	void HandleDidReceiveReward(object sender, RewardEventArgs e)
	#endif
	{
		ReceivedReward = true;
		ReceivedRewardIsSuccess =
			#if UNITY_ANDROID
			e.isSuccess;
			#else
			e.success;
			#endif

		print("HandleDidReceiveReward: ReceivedRewardIsSuccess = " + ReceivedRewardIsSuccess);
	}

	bool AdClosed;

	void HandleAdClosedEvent(object sender, EventArgs e)
	{
		print("AdMarvelView HandleAdClosedEvent");

		AdClosed = true;

	}

	bool FailedToLoad;

	void HandleInterstitialFailedToLoad(object sender, EventArgs e)
	{
		print("AdMarvelView HandleInterstitialFailedToLoad");

		FailedToLoad = true;
	}

	void HandleInterstitialLoaded(object sender, EventArgs e)
	{
		print("AdMarvelView HandleInterstitialLoaded");

		#if UNITY_ANDROID
		AdMarvelInterstitial.DisplayInterstitial();
		#else
		AdMarvelInterstitial.DisplayInterstitialAd();
		#endif
	}
}
