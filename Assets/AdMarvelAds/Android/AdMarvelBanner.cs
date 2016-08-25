using UnityEngine;
using System;
using AdMarvelAndroidAds;
using System.Collections;
using System.Collections.Generic;

namespace AdMarvelAndroidAds
{
	public class AdMarvelBanner : BannerAdListener {

		private AndroidJavaObject adMarvelBannerView;

		//These are the ad callback events that can be hooked into.
		public event EventHandler<EventArgs> AdMarvelAdReceived = delegate {};
		public event EventHandler<AdMarvelAdFailedToReceiveEventArgs> AdMarvelAdFailedToReceived = delegate {};
		public event EventHandler<EventArgs> AdMarvelAdClosed = delegate {};
		public event EventHandler<EventArgs> AdMarvelAdClicked = delegate {};
		public event EventHandler<EventArgs> AdMarvelAdRequested = delegate {};
		public event EventHandler<EventArgs> AdMarvelAdExpand = delegate {};

		// video events delegate...
		public event EventHandler<EventArgs> onAudioStop = delegate {};
		public event EventHandler<EventArgs> onAudioStart = delegate {};
		public event EventHandler<AdMarvelVideoEvent> onAdMarvelVideoEvent = delegate {};

		public AdMarvelBanner(){
			//getting current activity instance
			AndroidJavaClass playerClass = new AndroidJavaClass (AdMarvelUtils.UnityActivityClassName);
			AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");

			/* creating Android java plugin Banner Ads class object.(using available api class AndroidJavaObject )
				param1- activity instace(sending instance to constructor)
				param2- adlistener instance for fire cllback to unity form android java plugins.
			*/
			adMarvelBannerView = new AndroidJavaObject (AdMarvelUtils.UnityAdmarvelBannerAdsClassName,activity,new AdMarvelBannerAdListener(((BannerAdListener)this)),"-2");
		}


		/* request for banner ad
		 * params-:
		 * 		Dictionary -: for taking targetparams it will be as key value pair(this will be converted into java HashMap object.).
		 * 		patnerid & siteId-: needed for showing exact banner ad.
		 * 		adPosition-: banner ad position on the view, this will be define by publisher. 
		 */
		public void requestNewAd(Dictionary<string,string> Params,string partnerId, string siteId, AdMarvelAdPosition adPosition ){

			if (string.IsNullOrEmpty (partnerId) && string.IsNullOrEmpty (siteId)) {
					Debug.Log ("Unity-AdMarvelRequestAdError  : SITE_ID_OR_PARTNER_ID_NOT_PRESENT");		
			} else {
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
							Debug.Log ("Unity : requestNewAd");
							/*
								calling Android Java plugin method. 	
								param-1: method name in java class.
								param-2: parameters of method(if multiple method then we can pass making array)
							 */
							adMarvelBannerView.Call ("requestNewAd", new object[4]{targetParams,partnerId,siteId,(int)adPosition});
	
					}
			}

		}


		public void Show(){
			adMarvelBannerView.Call ("show");
		}

		public void Hide(){
			adMarvelBannerView.Call ("hide");
		}

		public void Destroy(){
			adMarvelBannerView.Call ("destroy");
		}



		#region AdListener implementation
		/*
			implemented AdListener methods for handling callbacks,
			also fired the call backs to publisher using delegate.
		 */
		void BannerAdListener.FireAdMarvelAdReceived(){
			AdMarvelAdReceived (this , EventArgs.Empty);
		}

		void BannerAdListener.FireAdMarvelAdFailedToReceive(string message){

			AdMarvelAdFailedToReceiveEventArgs args = new AdMarvelAdFailedToReceiveEventArgs ();
			args.Message = message;
			AdMarvelAdFailedToReceived (this , args);
		}

		void BannerAdListener.FireAdMarvelAdClosed(){
			AdMarvelAdClosed (this , EventArgs.Empty);
		}

		void BannerAdListener.FireAdMarvelAdClicked(){
			AdMarvelAdClicked (this , EventArgs.Empty);
		}

		void BannerAdListener.FireAdMarvelAdRequested(){
			AdMarvelAdRequested (this , EventArgs.Empty);
		}
		
		void BannerAdListener.FireAdMarvelAdExpanded(){
			AdMarvelAdExpand (this , EventArgs.Empty);
		}


		//Video events Callback
		
		void BannerAdListener.FireOnAudioStop(){
			
			onAudioStop (this , EventArgs.Empty);
		}
		
		void BannerAdListener.FireOnAudioStart(){
			
			onAudioStart (this , EventArgs.Empty);
		}
		
		void BannerAdListener.FireOnAdMarvelVideoEvent(string adMarvelVideoEvent){
			
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
		public void EnableLogging(bool enableLoggingFlag){
			adMarvelBannerView.Call ("enableLogging",enableLoggingFlag);
		}


		// public apis for banner view.

		/**
		 * Returns true if ad is in expanded state
		 * 
		 * @return
		 */
		public bool IsAdExpanded(){
			return adMarvelBannerView.Call<bool>("isAdExpanded");
		}

		/**
		 * Returns true if using Fetch-Display Model for serving Banner Ads
		 * 
		 * @return
		 */
		public bool IsAdFetchedModel(){
			return adMarvelBannerView.Call<bool>("isAdFetchedModel");
		}

		/**
		 * Set to true to disable animations such as rotation and fading. If you
		 * plan on using your own custom animations. Please disable animations.
		 * Otherwise the SDK will no be able to fetch new Ads because your
		 * animation will collide with the SDK's animations.
		 * 
		 * @param disableAnimation
		 *            boolean
		 */
		public void SetDisableAnimation(bool disableAnimation){
			adMarvelBannerView.Call("setDisableAnimation",disableAnimation);
		}

		/**
		 * Default is False, If set True SDK will depend on App to notify him
		 * for managing Impression Trackers
		 * 
		 * @param disableSDKImpressionTracking
		 */
		public void SetDisableSDKImpressionTracking(bool disableSDKImpressionTracking){
			adMarvelBannerView.Call("setDisableSDKImpressionTracking",disableSDKImpressionTracking);
		}

		/**
		 * Default is false. This flag is used while setting
		 * hardwareAcceleration Flag in Poststring and WebsettingInitilazer.
		 * 
		 * @param setSoftwareLayerFlag
		 */
		public void SetAdmarvelWebViewAsSoftwareLayer(bool setSoftwareLayerFlag){
			adMarvelBannerView.Call("setAdmarvelWebViewAsSoftwareLayer",setSoftwareLayerFlag);
		}

		/**
		 * Defaults to true, If set to true, SDK enables hardware acceleration
		 * to render inline video properly
		 * 
		 * @param enableHardwareAccelerationFlag
		 */
		public void SetEnableHardwareAcceleration(bool enableHardwareAccelerationFlag){
			adMarvelBannerView.Call("setEnableHardwareAcceleration",enableHardwareAccelerationFlag);
		}

		/**
		 * Set to true, if you want the image ads to fit to screen for tablet
		 * devices.
		 * 
		 * @param enableFitToScreenForTablets
		 * 
		 */
		public void SetEnableFitToScreenForTablets(bool enableFitToScreenForTablets){
			adMarvelBannerView.Call("setEnableFitToScreenForTablets",enableFitToScreenForTablets);
		}

		/**
		 * This method is used to Set Ad Container Width. Pass absolute pixels
		 * as width. For example, if expected width is 300 dip then pass
		 * 300*device_density.
		 * 
		 * @param adContainerWidth
		 * 
		 */
		public void SetAdContainerWidth(int adContainerWidth){
			adMarvelBannerView.Call("setAdContainerWidth",adContainerWidth);
		}

		/**
		 * Set the background color of the AdmarvelView in RGB format
		 * 
		 * @param backgroundColor
		 *            RGB integer value of a color
		 */
		public void SetAdMarvelBackgroundColor(int backgroundColor){
			adMarvelBannerView.Call("setAdMarvelBackgroundColor",backgroundColor);
		}	

		/**
		 * Set to true, if you want to auto-scale for high-res devices.
		 * 
		 * @param enableAutoScaling
		 * 
		 */

		public void SetEnableAutoScaling(bool enableAutoScaling){
			adMarvelBannerView.Call("setEnableAutoScaling",enableAutoScaling);
		}
		
		public void SetTextBackgroundColor(int textBackgroundColor){
			adMarvelBannerView.Call("setTextBackgroundColor",textBackgroundColor);
		}

		/**
		 * Set the text background color in RGB format of an AdMarvel Text Ad.
		 * 
		 * @param textBackgroundColor
		 *            RGB integer value of a color
		 */
		public void SetEnableClickRedirect(bool enableClickRedirect){
			adMarvelBannerView.Call("setEnableClickRedirect",enableClickRedirect);
		}

		/**
		 * Returns the background color of the AdmarvelView in ARGB format
		 * 
		 * @return background color of the AdmarvelView in ARGB format
		 * 
		 */
		public int GetAdMarvelBackgroundColor(){
			return adMarvelBannerView.Call<int>("getAdMarvelBackgroundColor");
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
			return adMarvelBannerView.Call<int>("getTextBackgroundColor");
		}

		/**
		 * Returns true if animations are disabled
		 * 
		 * @return true, if animations are disabled
		 * 
		 */
		public bool isDisableAnimation(){
			return adMarvelBannerView.Call<bool>("isDisableAnimation");
		}

		/**
		 * Returns true if fit to screen for tablets is enabled
		 * 
		 * @return
		 */
		public bool isEnableFitToScreenForTablets(){
			return adMarvelBannerView.Call<bool>("isEnableFitToScreenForTablets");
		}

		/**
		 * Returns true if auto scaling is enabled
		 * 
		 * @return
		 */
		public bool isAutoScalingEnabled(){
			return adMarvelBannerView.Call<bool>("isAutoScalingEnabled");
		}

		/**
		 * Defaults to true, If set to true, then open a browser when user
		 * clicks on an ad. This is only supported for Admarvel Ads
		 * 
		 * @return true, if redirect is enabled.
		 */
		public bool isEnableClickRedirect(){
			return adMarvelBannerView.Call<bool>("isEnableClickRedirect");
		}

		/**
		 * Returns the text font color in ARGB format of an AdMarvel Text Ad.
		 * 
		 * @return textFontColor background color in ARGB format of an AdMarvel
		 *         Text Ad.
		 * 
		 */
		public int getTextFontColor()
		{
			return adMarvelBannerView.Call<int>("getTextFontColor");
		}
		
		/**
		 * Returns the text border color in ARGB format of an AdMarvel Text Ad.
		 * 
		 * @return textBorderColor background color in ARGB format of an
		 *         AdMarvel Text Ad.
		 * 
		 */
		public int getTextBorderColor()
		{
			return adMarvelBannerView.Call<int>("getTextBorderColor");
		}

		/**
		 * Set to true, if you want to serve Banner Ads using Fetch-Display
		 * Model
		 * 
		 * @param isAdFetchedModel
		 * 
		 */
		public void enableAdFetchedModel( bool isAdFetchedModel )
		{
			adMarvelBannerView.Call("enableAdFetchedModel",isAdFetchedModel);
		}
		
		/**
		 * Returns true if current view is for Postitial Ads
		 * 
		 * @return
		 */
		public bool isPostitialView()
		{
			return adMarvelBannerView.Call<bool>("isPostitialView");
		}

		/**
		 * Set to true for using current view for Postitial Ads
		 * 
		 * @param isPostitialView
		 * 
		 */
		public void setPostitialView( bool isPostitialView )
		{
			adMarvelBannerView.Call("setPostitialView",isPostitialView);
		}


		/**
		 * Set the text font color in RGB format of an AdMarvel Text Ad.
		 * 
		 * @param textFontColor
		 *            RGB integer value of a color
		 */
		public void setTextFontColor( int textFontColor )
		{
			adMarvelBannerView.Call("setTextFontColor",textFontColor);
		}
		
		/**
		 * Set the text border color in RGB format of an AdMarvel Text Ad.
		 * 
		 * @param textBorderColor
		 *            RGB integer value of a color
		 */
		public void setTextBorderColor( int textBorderColor )
		{
			adMarvelBannerView.Call("setTextBorderColor",textBorderColor);
		}

	}


	class AdMarvelBannerAdListener : AndroidJavaProxy
	{
		private BannerAdListener listener;
		
		internal AdMarvelBannerAdListener(BannerAdListener listener): base(AdMarvelUtils.UnityAdMarvelAdListenerClassName){
			
			this.listener = listener;
		}
		
		void onReceiveAd(){
			Debug.Log("Unity : onReceiveAd");
			listener.FireAdMarvelAdReceived ();
		}
		
		void onFailedToReceiveAd(string errorReason){
			Debug.Log("Unity : onFailedToReceiveAd : "+ errorReason);
			listener.FireAdMarvelAdFailedToReceive (errorReason);
		}
		
		void onClosedAd(){
			Debug.Log("Unity : onClosedAd");
			listener.FireAdMarvelAdClosed ();
		}
		
		void onClickedAd(){
			Debug.Log("Unity : onClickedAd");
			listener.FireAdMarvelAdClicked ();
		}
		
		void onRequestAd(){
			Debug.Log("Unity : onRequestAd");
			listener.FireAdMarvelAdRequested ();
		}
		
		void onExpand(){
			Debug.Log("Unity : onExpand");
			listener.FireAdMarvelAdExpanded ();
		}
		void onAudioStop(){
			Debug.Log("Unity : onAudioStop");
			listener.FireOnAudioStop ();
		}
		void onAudioStart(){
			Debug.Log("Unity : onAudioStart");
			listener.FireOnAudioStart ();
		}
		
		void onAdMarvelVideoEvent(string adMarvelVideoEvent){
			Debug.Log("Unity : onAdMarvelVideoEvent : "+ adMarvelVideoEvent);
			listener.FireOnAdMarvelVideoEvent (adMarvelVideoEvent);
		}
	}
	

	internal interface BannerAdListener
	{		
		void FireAdMarvelAdReceived();
		void FireAdMarvelAdFailedToReceive(string errorReason);
		void FireAdMarvelAdClosed();
		void FireAdMarvelAdClicked();
		void FireAdMarvelAdRequested();
		void FireAdMarvelAdExpanded();

		void FireOnAudioStop();
		void FireOnAudioStart();
		void FireOnAdMarvelVideoEvent(string adMarvelVideoEvent  );
	}

	
}