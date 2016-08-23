using UnityEngine;
using System;
using AdMarvelAndroidAds;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace AdMarvelAndroidAds
{
	
	public class AdMarvelInterstitial : InterstitialAdListener {

		private AndroidJavaObject adMarvelInterstitialView;

		//These are the ad callback events that can be hooked into.
		public event EventHandler<EventArgs> AdMarvelInterstitialAdReceived = delegate {};
		public event EventHandler<AdMarvelInterstitialAdFailedToReceive> AdMarvelInterstitialAdFailedToReceived = delegate {};
		public event EventHandler<EventArgs> AdMarvelInterstitialAdClosed = delegate {};
		public event EventHandler<EventArgs> AdMarvelInterstitialAdClicked = delegate {};
		public event EventHandler<EventArgs> AdMarvelInterstitialAdRequested = delegate {};
		public event EventHandler<AdMarvelInterstitialObjectEvent> AdmarvelActivityLaunched = delegate {};
		public event EventHandler<AdMarvelInterstitialObjectEvent> AdMarvelVideoActivityLaunched = delegate {};
		public event EventHandler<EventArgs> AdMarvelInterstitialAdDisplayed = delegate {};

		// video events delegate...
		public event EventHandler<EventArgs> onAudioStop = delegate {};
		public event EventHandler<EventArgs> onAudioStart = delegate {};
		public event EventHandler<AdMarvelVideoEvent> onAdMarvelVideoEvent = delegate {};

		public event EventHandler<AdMarvelRewardAdResultEvent> AdMarvelRewardAdResult = delegate {};

		public AdMarvelInterstitial(){

			//getting current activity instance
			AndroidJavaClass playerClass = new AndroidJavaClass (AdMarvelUtils.UnityActivityClassName);
			AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");

			/* creating Android java plugin interstitial Ads class object.(using available api class AndroidJavaObject )
				param1- activity instace(sending instance to constructor)
				param2- adlistener instance for fire cllback to unity form android java plugins.
			*/
			adMarvelInterstitialView = new AndroidJavaObject (AdMarvelUtils.UnityAdmarvelInterstitialAdsClassName,activity,new AdMarvelInterstitialAdListener(((InterstitialAdListener)this)));
		}


		/* request for interstitial ad
		 * params-:
		 * 		Dictionary -: for taking targetparams it will be as key value pair(this will be converted into java HashMap object.).
		 * 		patnerid & interstitialSiteId-: needed for showing exact interstitial ad.
		 */
		public void requestNewInterstitialAd(Dictionary<string,string> Params,string partnerId, string interstitialSiteId ){

			if (string.IsNullOrEmpty (partnerId) && string.IsNullOrEmpty (interstitialSiteId)) {
				Debug.Log ("Unity-AdMarvelRequestAdError  : SITE_ID_OR_PARTNER_ID_NOT_PRESENT");		
			}else{
				//converting Dictionary object into the Android java HAshMap class object.
				using (AndroidJavaObject targetParams = new AndroidJavaObject(AdMarvelUtils.HashMapClassName)) {
					if (Params != null && Params.Count > 0) {
								IntPtr method_Put = AndroidJNIHelper.GetMethodID (targetParams.GetRawClass (), "put",
	                                                 "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

								object[] args = new object[2];
								foreach (KeyValuePair<string, string> kvp in Params) {//retrving key/value paire form dictionary object
										using (AndroidJavaObject k = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Key)) {
												using (AndroidJavaObject v = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Value)) {
														args [0] = k;
														args [1] = v;
														//inserting key value pair into the java hashmap object.
														AndroidJNI.CallObjectMethod (targetParams.GetRawObject (),
				                            method_Put, AndroidJNIHelper.CreateJNIArgArray (args));
												}
										}
								}
						} 
						Debug.Log ("Unity : requestNewInterstitialAd");				
						adMarvelInterstitialView.Call ("requestNewInterstitialAd", new object[3] {
						targetParams,
						partnerId,
						interstitialSiteId
				});
			
				}
			} 
		}


		public void requestRewardInterstitial(Dictionary<string,string> Params,string partnerId, string interstitialSiteId, Dictionary<string,string> rewardParams ){

			if (string.IsNullOrEmpty (partnerId) && string.IsNullOrEmpty (interstitialSiteId)) {
					Debug.Log ("Unity-AdMarvelRequestAdError  : SITE_ID_OR_PARTNER_ID_NOT_PRESENT");		
			} else {
					//converting Dictionary object into the Android java HAshMap class object.
					using (AndroidJavaObject targetParams = new AndroidJavaObject(AdMarvelUtils.HashMapClassName),rewardAdParams = new AndroidJavaObject(AdMarvelUtils.HashMapClassName)) {
							object[] args = null;

							if (Params != null && Params.Count > 0) {
									IntPtr method_Put = AndroidJNIHelper.GetMethodID (targetParams.GetRawClass (), "put",
		                                                 "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		
									args = new object[2];
									foreach (KeyValuePair<string, string> kvp in Params) {//retrving key/value paire form dictionary object
											using (AndroidJavaObject k = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Key)) {
													using (AndroidJavaObject v = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Value)) {
															args [0] = k;
															args [1] = v;
															//inserting key value pair into the java hashmap object.
															AndroidJNI.CallObjectMethod (targetParams.GetRawObject (),
					                            method_Put, AndroidJNIHelper.CreateJNIArgArray (args));
													}
											}
									}
							}

							if (rewardParams != null && rewardParams.Count > 0) {
									IntPtr method_Put_reward = AndroidJNIHelper.GetMethodID (rewardAdParams.GetRawClass (), "put",
		                                                  "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		
									args = new object[2];
									foreach (KeyValuePair<string, string> kvp in Params) {//retrving key/value paire form dictionary object
											using (AndroidJavaObject k = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Key)) {
													using (AndroidJavaObject v = new AndroidJavaObject(AdMarvelUtils.StringClassName, kvp.Value)) {
															args [0] = k;
															args [1] = v;
															//inserting key value pair into the java hashmap object.
															AndroidJNI.CallObjectMethod (rewardAdParams.GetRawObject (),
						                             method_Put_reward, AndroidJNIHelper.CreateJNIArgArray (args));
													}
											}
									}
							}

							Debug.Log ("Unity : requestRewardInterstitial");	
							adMarvelInterstitialView.Call ("requestRewardInterstitial", new object[4] {
									targetParams,
									partnerId,
									interstitialSiteId,
									rewardAdParams
							});


					}
			}
		}


		public void DisplayInterstitial(){
			Debug.Log("Unity : DisplayInterstitial");	
			adMarvelInterstitialView.Call ("displayInterstitial");
		}

		public void Destroy(){
			adMarvelInterstitialView.Call ("destroy");
		}



		#region InterstitialAdListener implementation

		void InterstitialAdListener.FireAdMarvelInterstitialAdReceived(){
			AdMarvelInterstitialAdReceived (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireAdMarvelInterstitialAdFailedToReceive(string message){

			AdMarvelInterstitialAdFailedToReceive args = new AdMarvelInterstitialAdFailedToReceive ();
			args.Message = message;
			AdMarvelInterstitialAdFailedToReceived (this , args);
		}

		void InterstitialAdListener.FireAdMarvelInterstitialAdClosed(){
			AdMarvelInterstitialAdClosed (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireAdMarvelInterstitialAdClicked(){
			AdMarvelInterstitialAdClicked (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireAdMarvelInterstitialAdRequested(){

			AdMarvelInterstitialAdRequested (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireAdmarvelActivityLaunched(AndroidJavaObject a){

			AdMarvelInterstitialObjectEvent args = new AdMarvelInterstitialObjectEvent ();
			args.AndroidActivity = a;
			AdmarvelActivityLaunched (this ,args);
		}

		void InterstitialAdListener.FireAdMarvelVideoActivityLaunched(AndroidJavaObject a){

			AdMarvelInterstitialObjectEvent args = new AdMarvelInterstitialObjectEvent ();
			args.AndroidActivity = a;
			AdMarvelVideoActivityLaunched (this , args);
		}

		void InterstitialAdListener.FireAdMarvelInterstitialAdDisplayed(){
			
			AdMarvelInterstitialAdDisplayed (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireAdMarvelRewardAdResult(AndroidJavaObject rewardAdResult){

			AdMarvelRewardAdResultEvent args = new AdMarvelRewardAdResultEvent ();
			args.isSuccess = rewardAdResult.Call<Boolean>("isSuccess");
			args.RewardName = rewardAdResult.Call<string>("getRewardName");
			args.RewardValue = rewardAdResult.Call<string>("getRewardValue");
			AdMarvelRewardAdResult (this , args);

		}
	
		//Video events Callback

		void InterstitialAdListener.FireOnAudioStop(){
			
			onAudioStop (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireOnAudioStart(){
			
			onAudioStart (this , EventArgs.Empty);
		}

		void InterstitialAdListener.FireOnAdMarvelVideoEvent(string adMarvelVideoEvent){


			AdMarvelVideoEvent args = new AdMarvelVideoEvent ();
			Dictionary<string,string> customeEventParamsArgs = null;

			if(adMarvelVideoEvent!=null){
				try{
					string []eventnameValue= adMarvelVideoEvent.Split('|');
					if(eventnameValue.Length>0){
						AdMarvelVideoEvents adMarvelVideoEventArg =  AdMarvelUtils.getEnum (eventnameValue[0]);
						args.adMarvelVideoEvent = adMarvelVideoEventArg;
						customeEventParamsArgs = new Dictionary<string,string> ();
						customeEventParamsArgs.Add (eventnameValue[1],eventnameValue[2]);
						args.customEventParams = customeEventParamsArgs;	
					}
				}catch(Exception ex){
					Debug.Log(ex.Message);
				}
			}

					
			onAdMarvelVideoEvent (this , args);
			
		}
		#endregion


		//Public APIs...

		
		/***
		 * This Api is used to enable AdMavel SDK logs .
		 * 
		 * Note:- Logging can also be enabled from command line using the
		 * command " adb shell setprop log.tag.admarvel VERBOSE "
		 * 
		 * @param enableLoggingFlag
		 *            :- a boolean flag true to enable logging and false to
		 *            disable. default logging is disabled
		 * 
		 * 
		 */
		public void EnableLogging(Boolean enableLoggingFlag){
			adMarvelInterstitialView.Call ("enableLogging",enableLoggingFlag);
		}


		/**
		 * Set the background color of the AdmarvelView in RGB format
		 * 
		 * @param backgroundColor
		 *            RGB integer value of a color
		 */

		public void SetAdMarvelBackgroundColor(int backgroundColor){
			adMarvelInterstitialView.Call ("setAdMarvelBackgroundColor",backgroundColor);
		}	

		/**
		 * Set to true, if you want to auto-scale for high-res devices.
		 * 
		 * @param enableAutoScaling
		 * 
		 */
		public void SetEnableAutoScaling(bool enableAutoScaling){
			adMarvelInterstitialView.Call ("setEnableAutoScaling",enableAutoScaling);
		}
		
//		public void setOptionalFlags(Map<String,String> optionalFlags){
//			adMarvelInterstitialAds.setOptionalFlags(optionalFlags);
//			adMarvelInterstitialView.Call ("enableLogging",enableLoggingFlag);
//		}	

		/**
		 * Set the text background color in RGB format of an AdMarvel Text Ad.
		 * 
		 * @param textBackgroundColor
		 *            RGB integer value of a color
		 */
		public void SetTextBackgroundColor(int textBackgroundColor){
			adMarvelInterstitialView.Call ("setTextBackgroundColor",textBackgroundColor);
		}

		/**
		 * Defaults to true, If set to true, then open a browser when user
		 * clicks on an ad. Set this for false if you want to handle the click
		 * url yourself.
		 * 
		 * @param enableClickRedirect
		 */
		public void SetEnableClickRedirect(bool enableClickRedirect){
			adMarvelInterstitialView.Call ("setEnableClickRedirect",enableClickRedirect);
		}
			
		/**
		 * Returns the background color of the AdmarvelView in ARGB format
		 * 
		 * @return background color of the AdmarvelView in ARGB format
		 * 
		 */
		public int GetAdMarvelBackgroundColor(){
			return adMarvelInterstitialView.Call<int>("getAdMarvelBackgroundColor");
		}	

		/**
		 * Returns the text background color in ARGB format of an AdMarvel Text
		 * Ad.
		 * 
		 * @return textBackgroundColor background color in ARGB format of an
		 *         AdMarvel Text Ad.
		 * 
		 */
		public int GetTextBackgroundColor(){
			return adMarvelInterstitialView.Call<int>("getTextBackgroundColor");
		}

		/**
		 * Returns if click redirect is enabled/disabled
		 * 
		 * @return
		 */
		public bool GetEnableClickRedirect( ){
			return adMarvelInterstitialView.Call<bool>("getEnableClickRedirect");
		}


	}

	internal interface InterstitialAdListener
	{
		void FireAdMarvelInterstitialAdReceived();
		void FireAdMarvelInterstitialAdFailedToReceive(string errorReason);
		void FireAdMarvelInterstitialAdClosed();
		void FireAdMarvelInterstitialAdClicked();
		void FireAdMarvelInterstitialAdRequested();
		void FireAdmarvelActivityLaunched (AndroidJavaObject a);		
		void FireAdMarvelVideoActivityLaunched(AndroidJavaObject a);
		void FireAdMarvelInterstitialAdDisplayed();
		void FireOnAudioStop();
		void FireOnAudioStart();
		void FireOnAdMarvelVideoEvent(string adMarvelVideoEvent);
		void FireAdMarvelRewardAdResult(AndroidJavaObject adMarvelReward);
	}


	class AdMarvelInterstitialAdListener : AndroidJavaProxy
	{
		private InterstitialAdListener listener;
		
		internal AdMarvelInterstitialAdListener(InterstitialAdListener listener): base(AdMarvelUtils.UnityAdMarveAdInterstitialListenerClassName){
			
			this.listener = listener;
		}
		
		void onReceiveInterstitialAd(){
			Debug.Log("Unity : onReceiveInterstitialAd");	
			Debug.Log("Unity : onReceiveInterstitialAd ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 

			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(runOnUiThread2));
		}

		void runOnUiThread2() {
			listener.FireAdMarvelInterstitialAdReceived ();
		}
		
		void onFailedToReceiveInterstitialAd(string errorReason){
			Debug.Log("Unity : onFailedToReceiveInterstitialAd" + errorReason);	
			listener.FireAdMarvelInterstitialAdFailedToReceive (errorReason);
		}
		
		void onCloseInterstitialAd(){
			Debug.Log("Unity : onCloseInterstitialAd");	
			Debug.Log("Unity : onCloseInterstitialAd ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 



			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(runOnUiThread));

		}

		void runOnUiThread() {
			listener.FireAdMarvelInterstitialAdClosed ();
		}
		
		void onClickInterstitialAd(){
			Debug.Log("Unity : onClickInterstitialAd");	
			Debug.Log("Unity : onClickInterstitialAd ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 

			listener.FireAdMarvelInterstitialAdClicked ();

		}
		
		void onRequestInterstitialAd(){
			Debug.Log("Unity : onRequestInterstitialAd");	
			Debug.Log("Unity : onRequestInterstitialAd ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 

			listener.FireAdMarvelInterstitialAdRequested ();
		}
		
		void onAdmarvelActivityLaunched(AndroidJavaObject a){
			Debug.Log("Unity : onAdmarvelActivityLaunched");	
			listener.FireAdmarvelActivityLaunched (a);
		}
		
		void onAdMarvelVideoActivityLaunched(AndroidJavaObject a){
			Debug.Log("Unity : onAdMarvelVideoActivityLaunched");	
			listener.FireAdMarvelVideoActivityLaunched (a);
		}

		void onInterstitialDisplayed(){
			Debug.Log("Unity : onInterstitialDisplayed");	
			Debug.Log("Unity : onInterstitialDisplayed ; current thread id: " + Thread.CurrentThread.GetHashCode().ToString()); 

			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			activity.Call("runOnUiThread", new AndroidJavaRunnable(runOnUiThread3));
		}

		void runOnUiThread3() {
			listener.FireAdMarvelInterstitialAdDisplayed ();
		}

		void onReward( AndroidJavaObject adMarvelReward ){
			Debug.Log("Unity : onReward");	
			listener.FireAdMarvelRewardAdResult (adMarvelReward);
		}
		void onAudioStop(){
			Debug.Log("Unity : onAudioStop");	
			listener.FireOnAudioStop ();
		}
		void onAudioStart(){
			Debug.Log("Unity : onAudioStart");	
			listener.FireOnAudioStart ();
		}

		void onAdMarvelVideoEvent(string adMarvelVideoEvent ){
			Debug.Log("Unity : onAdMarvelVideoEvent" + adMarvelVideoEvent);	
			listener.FireOnAdMarvelVideoEvent (adMarvelVideoEvent);
		}

	}

}