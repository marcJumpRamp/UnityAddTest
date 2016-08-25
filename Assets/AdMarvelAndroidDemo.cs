using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdMarvelAndroidAds;
using System;
using System.Threading;
using UnityEngine.UI;


public class AdMarvelAndroidDemo : MonoBehaviour {

	private AdMarvelBanner adMarvelBannerView;
	private AdMarvelInterstitial adMarvelInterstitialView;
	
	private bool ishideview = false;//flag for demo application.
	public string Info;

	public string patnerID = "100b70a8a2248717";
	public string siteId = "55129";
	public string targetpKey = "targetParamKey";
	public string targetPValue = "targetParamValue";

	static bool isShowInterstitialFlag = false;
	private Button baseButton;


	// Use this for initialization
	void Start ()
	{
//		baseButton = GetComponent<Button>();
////		if(onClick != null)
////			baseButton.onClick = onClick;
//
//		//update label
//		baseButton.GetComponentInChildren<Text>().text = "button 1";
		Initialize (); //initializing adnetworks on app launch...
	}
	
	void OnGUI()
	{
		// Puts some basic buttons onto the screen.
		GUI.skin.button.fontSize = (int)(0.05f * Screen.height);

		if (!ishideview) {
					

						GUI.skin.textField.fontSize = (int)(0.02f * Screen.height);
						Rect patnerid = new Rect (0.1f * Screen.width, 0.05f * Screen.height,
			                          0.8f * Screen.width, 0.050f * Screen.height);
						patnerID = GUI.TextField(patnerid, patnerID, 50);
						
						Rect siteidrect = new Rect(0.1f * Screen.width, 0.120f * Screen.height,
			                           0.8f * Screen.width, 0.050f * Screen.height);
						
						siteId = GUI.TextField(siteidrect, siteId, 50);
						
						Rect targetpk = new Rect(0.1f * Screen.width, 0.190f * Screen.height,
			                         0.8f * Screen.width, 0.050f * Screen.height);
						
						targetpKey = GUI.TextField(targetpk,targetpKey, 50);
						
						Rect targetpv = new Rect(0.1f * Screen.width, 0.260f * Screen.height,
						                         0.8f * Screen.width, 0.050f * Screen.height);
						targetPValue = GUI.TextField(targetpv,targetPValue,50);
						
						//button for request for banner ads...
						Rect requestBannerRect = new Rect (0.1f * Screen.width, 0.340f * Screen.height,
		                                  0.8f * Screen.width, 0.1f * Screen.height);
						if (GUI.Button (requestBannerRect, "Request Banner")) {
								RequestBanner ();
						}
		
						//button for request for interstitial ads...
						Rect requestInterstitial = new Rect (0.1f * Screen.width, 0.465f * Screen.height,
		                                    0.8f * Screen.width, 0.1f * Screen.height);
						if (GUI.Button (requestInterstitial, "Request Interstitial")) {
								isShowInterstitialFlag = false;
								RequestInterstitial ();
						}

						//button for request for reward ads...
						Rect requestForRewardInterstitial = new Rect (0.1f * Screen.width, 0.590f * Screen.height,
						                                     0.8f * Screen.width, 0.1f * Screen.height);
						if (GUI.Button (requestForRewardInterstitial, "Request RewardAd")) {
								isShowInterstitialFlag = false;
								RequestRewardInterstitialAd();
						}	
						

				}

				//button for display interstitial & reward ads...
				if (isShowInterstitialFlag) {
					Rect displayInterstitial = new Rect (0.1f * Screen.width, 0.710f * Screen.height,
					                                     0.8f * Screen.width, 0.1f * Screen.height);
					if (GUI.Button (displayInterstitial, "Display Interstitial")) {
						if (adMarvelInterstitialView != null)
							adMarvelInterstitialView.DisplayInterstitial (); // this api is used for the showing interstitial and reward ads.
					}	
				}

	}	

//	public void onClick()
//	{
//	}


	// call initialize method after activity is started.
	public void Initialize(){

		Dictionary<string,string> publisherIds = new Dictionary<string, string>();
		publisherIds.Add (AdMarvelSDKNetworks.INMOBI ,"9d3904ff01e24860bcfbbce28fb3644f" );
		publisherIds.Add (AdMarvelSDKNetworks.HEYZAP ,"c592f1ff4887e933f926050cfa2c2e96" );
		publisherIds.Add (AdMarvelSDKNetworks.ADCOLONY ,"1.0|app8427068c88194afaa7|vz309c42695fa849bfa9|vza6ed56147cdb43cc86|vz15447e55a0f746e78f|vz489065236c604310b1|vz30b15efc02314512ae|vzfbf7b82a222e4b2c84|vz9bd66be4333b4ec4b3|vz18981b122acc4a9395" );
		publisherIds.Add (AdMarvelSDKNetworks.UNITYADS ,"23036" );
		publisherIds.Add (AdMarvelSDKNetworks.YUME ,"211EsvNSRHO|http://shadow01.yumenetworks.com" );//(<YuMeAppId>|<YuMeAdURL>)
		publisherIds.Add (AdMarvelSDKNetworks.VUNGLE ,"55a042f16ae8fe471b00000a" );
		publisherIds.Add (AdMarvelSDKNetworks.CHARTBOOST ,"4f7b433509b6025804000002|dd2d41b69ac01b80f443f5b6cf06096d457f82bd" );//(<cbAppId>|<cbAppSignature>)

		AdMarvelUtils.initialize (publisherIds);

		makeToast("Initialize");

	}


	//request for AdMarvel Banner Ad.
	private void RequestBanner()
	{
		print ("Unity RequestBanner called");

		string patnerId = patnerID; // add your patnerID
		string siteid = siteId; // add your siteId.

		if(adMarvelBannerView!=null){
			adMarvelBannerView.Destroy ();
		}

		// Create a banner object.
		adMarvelBannerView = new AdMarvelBanner ();

		
		// Register for ad events.
		adMarvelBannerView.AdMarvelAdClicked += HandleAdMarvelAdClicked;
		adMarvelBannerView.AdMarvelAdFailedToReceived += HandleAdMarvelAdFailedToReceived;
		adMarvelBannerView.AdMarvelAdClosed += HandleAdMarvelAdClosed;
		adMarvelBannerView.AdMarvelAdExpand += HandleAdMarvelAdExpand;
		adMarvelBannerView.AdMarvelAdReceived += HandleAdMarvelAdReceived;
		adMarvelBannerView.AdMarvelAdRequested += HandleAdMarvelAdRequested;

		//callback for video ads...
		
		adMarvelBannerView.onAudioStart += HandleonAudioStart;
		adMarvelBannerView.onAudioStop += HandleonAudioStop;
		adMarvelBannerView.onAdMarvelVideoEvent += HandleonAdMarvelVideoEvent;

		//custome targetParams
		Dictionary<string,string> targetParams = new Dictionary<string, string>();
		targetParams.Add(targetpKey, targetPValue);


		adMarvelBannerView.SetEnableClickRedirect (true);
		adMarvelBannerView.SetDisableAnimation (false);


		// request a banner ad.
		adMarvelBannerView.requestNewAd(targetParams,patnerId,siteid,AdMarvelAdPosition.BottomCenter);


	}

	void HandleAdMarvelAdRequested (object sender, EventArgs e)
	{
		print("HandleAdMarvelAdRequested event received.");

		makeToast("onRequestAd");

	}
	
	void HandleAdMarvelAdReceived (object sender, EventArgs e)
	{
		print("HandleAdMarvelAdReceived event received.");
		makeToast("onReceiveAd");
	}
	
	void HandleAdMarvelAdExpand (object sender, EventArgs e)
	{
		print("HandleAdMarvelAdExpand event received.");
		makeToast("onExpand");
		ishideview = true;
	}
	
	void HandleAdMarvelAdClosed (object sender, EventArgs e)
	{
		print("HandleAdMarvelAdClosed event received.");
		Debug.Log("Unity : HandleAdMarvelAdClosed");
		makeToast("onClosedAd");
		ishideview = false;
	}
	
	void HandleAdMarvelAdFailedToReceived (object sender, AdMarvelAdFailedToReceiveEventArgs args)
	{
		print("HandleAdMarvelAdFailedToReceived event received with message: " + args.Message);
		makeToast("onFailedToReceiveAd: "+args.Message);
	}
	
	void HandleAdMarvelAdClicked (object sender, EventArgs e)
	{
		print("HandleAdMarvelAdClicked event received.");
		makeToast("onClickedAd");
	}


//---------------------------------------------------------------------------------------	

	//request interstitial Ad
	private void RequestInterstitial()
	{
		print ("Unity RequestInterstitial called");
		Debug.Log("Unity : RequestInterstitial ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 



		string patnerId =patnerID; // add your patnerID
		string siteid = siteId; // add your siteId.


		// crate object of AdMarvelInterstitialView.
		adMarvelInterstitialView = new AdMarvelInterstitial ();


		
		// Register for interstitial ad events.
		adMarvelInterstitialView.AdMarvelInterstitialAdClicked += HandleAdMarvelInterstitialAdClicked;
		adMarvelInterstitialView.AdMarvelInterstitialAdFailedToReceived += HandleAdMarvelInterstitialAdFailedToReceived;
		adMarvelInterstitialView.AdMarvelInterstitialAdClosed += HandleAdMarvelInterstitialAdClosed;
		adMarvelInterstitialView.AdMarvelInterstitialAdReceived += HandleAdMarvelInterstitialAdReceived;
		adMarvelInterstitialView.AdMarvelInterstitialAdRequested += HandleAdMarvelInterstitialAdRequested;
		adMarvelInterstitialView.AdmarvelActivityLaunched += HandleAdmarvelActivityLaunched;
		adMarvelInterstitialView.AdMarvelVideoActivityLaunched += HandleAdMarvelVideoActivityLaunched;
		adMarvelInterstitialView.AdMarvelInterstitialAdDisplayed += HandleAdMarvelInterstitialAdDisplayed;

		//callback for video ads...
		
		adMarvelInterstitialView.onAudioStart += HandleonAudioStart;
		adMarvelInterstitialView.onAudioStop += HandleonAudioStop;
		adMarvelInterstitialView.onAdMarvelVideoEvent += HandleonAdMarvelVideoEvent;


		//custome targetParams
		Dictionary<string,string> targetParams = new Dictionary<string, string>();
		targetParams.Add(targetpKey, targetPValue);



		// request for interstitial ad.
		adMarvelInterstitialView.requestNewInterstitialAd(targetParams,patnerId,siteid);
	}



	//request for reward  interstitial Ad
	private void RequestRewardInterstitialAd()
	{
		print ("Unity RequestRewardInterstitialAd called");
		
		string patnerId =patnerID; // add your patnerID
		string siteid = siteId; // add your siteId.
		
		
		// crate object of AdMarvelInterstitialView.
		adMarvelInterstitialView = new AdMarvelInterstitial ();
		
		// Register for interstitial ad events.
		adMarvelInterstitialView.AdMarvelInterstitialAdClicked += HandleAdMarvelInterstitialAdClicked;
		adMarvelInterstitialView.AdMarvelInterstitialAdFailedToReceived += HandleAdMarvelInterstitialAdFailedToReceived;
		adMarvelInterstitialView.AdMarvelInterstitialAdClosed += HandleAdMarvelInterstitialAdClosed;
		adMarvelInterstitialView.AdMarvelInterstitialAdReceived += HandleAdMarvelInterstitialAdReceived;
		adMarvelInterstitialView.AdMarvelInterstitialAdRequested += HandleAdMarvelInterstitialAdRequested;
		adMarvelInterstitialView.AdmarvelActivityLaunched += HandleAdmarvelActivityLaunched;
		adMarvelInterstitialView.AdMarvelVideoActivityLaunched += HandleAdMarvelVideoActivityLaunched;
		adMarvelInterstitialView.AdMarvelInterstitialAdDisplayed += HandleAdMarvelInterstitialAdDisplayed;

		//Use this callback for reward Ads result
		adMarvelInterstitialView.AdMarvelRewardAdResult += HandleAdMarvelRewardAdResult;

		//callback for video ads...

		adMarvelInterstitialView.onAudioStart += HandleonAudioStart;
		adMarvelInterstitialView.onAudioStop += HandleonAudioStop;
		adMarvelInterstitialView.onAdMarvelVideoEvent += HandleonAdMarvelVideoEvent;

		
		//custome targetParams
		Dictionary<string,string> targetParams = new Dictionary<string, string>();
		targetParams.Add(targetpKey, targetPValue);

		// Set reward specific properties and make ad request
		//optional reward ad Params
		Dictionary<string,string> rewardParams = new Dictionary<string, string>();
		
		
		// request for reward interstitial ad.
		adMarvelInterstitialView.requestRewardInterstitial(targetParams,patnerId,siteid,rewardParams);
	}
	

	void HandleAdMarvelInterstitialAdRequested (object sender, EventArgs e)
	{
		print("HandleAdMarvelInterstitialAdRequested event received.");
		makeToast("onInterstitialAdRequested");

	}
	
	void HandleAdMarvelInterstitialAdReceived (object sender, EventArgs e)
	{
		print("HandleAdMarvelInterstitialAdReceived event received.");
		Debug.Log("Unity : HandleAdMarvelInterstitialAdReceived ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 
		isShowInterstitialFlag = true;
		makeToast("onInterstitialAdReceived");
	}

	void HandleAdMarvelInterstitialAdClosed (object sender, EventArgs e)
	{
		print("HandleAdMarvelInterstitialAdClosed event received.");
		Debug.Log("Unity : HandleAdMarvelInterstitialAdClosed ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 
		isShowInterstitialFlag = false;
		makeToast("onInterstitialAdClosed");
//		baseButton.GetComponentInChildren<Text>().text = "akash2";

//		Rect requestInterstitial = new Rect (0.1f * Screen.width, 0.465f * Screen.height,
//			0.8f * Screen.width, 0.1f * Screen.height);
//		if (GUI.Button (requestInterstitial, "Changed Request Interstitial")) {
//			isShowInterstitialFlag = false;
//			RequestInterstitial ();
//		}

	}
	
	void HandleAdMarvelInterstitialAdFailedToReceived (object sender, AdMarvelInterstitialAdFailedToReceive args)
	{
		print("HandleAdMarvelInterstitialAdFailedToReceived event received with message: " + args.Message);
		makeToast("onInterstitialAdFailedToReceived");
	}
	
	void HandleAdMarvelInterstitialAdClicked (object sender, EventArgs e)
	{
		print("HandleAdMarvelInterstitialAdClicked event received.");
		makeToast("onInterstitialAdClicked");
	}

	void HandleAdmarvelActivityLaunched (object sender, AdMarvelInterstitialObjectEvent args)
	{
//		AndroidJavaObject a = args.AndroidActivity;
		print("HandleAdmarvelActivityLaunched event received.");
		makeToast("onAdmarvelActivityLaunched");
	}

	void HandleAdMarvelInterstitialAdDisplayed(object sender, EventArgs e){

		print("HandleAdMarvelInterstitialAdDisplayed event received.");
		makeToast("onAdMarvelInterstitialAdDisplayed");
	}

	void HandleAdMarvelVideoActivityLaunched (object sender, AdMarvelInterstitialObjectEvent args)
	{
//				AndroidJavaObject a = args.AndroidActivity;
		print("HandleAdMarvelVideoActivityLaunched event received.");
		makeToast("onAdMarvelVideoActivityLaunched");
	}

	void HandleAdMarvelRewardAdResult(object sender, AdMarvelRewardAdResultEvent args){
		if (args.isSuccess) {

			print( "admarvel" +
			      " adMarvelReward: rewardName - "
			      + args.RewardName
			      + " rewardValue - "
			      + args.RewardValue );
			makeToast( "admarvel" +
			          " adMarvelReward: rewardName - "
			          + args.RewardName
			          + " rewardValue - "
			          + args.RewardValue);
		} else {
			print( "admarvel" +" adMarvelReward: FAIL" );
			makeToast("adMarvelReward: FAIL" );
		}

	}


	//Video ad callbacks
	void HandleonAudioStart(object sender, EventArgs e){
		print("HandleonAudioStart event received.");
		makeToast("HandleonAudioStart");
	}

	void HandleonAudioStop(object sender, EventArgs e){
		print("HandleonAudioStop event received.");
		makeToast("HandleonAudioStop");
	}

	//Video ad events
	void HandleonAdMarvelVideoEvent(object sender, AdMarvelVideoEvent args){

		if (args.customEventParams != null)
			print ("HandleonAdMarvelVideoEvent - CUSTOM-: " +args.customEventParams["eventName"] );

		print( "HandleonAdMarvelVideoEvent" + args.adMarvelVideoEvent.ToString());
		makeToast("HandleonAdMarvelVideoEvent: " + args.adMarvelVideoEvent.ToString());
	}

	//use api for show notification...
	public static void makeToast(string toast)
	{
			AdMarvelUtils.showToast(toast);
	}
}

