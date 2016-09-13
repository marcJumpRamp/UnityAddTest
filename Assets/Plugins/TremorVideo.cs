
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class TremorVideo : MonoBehaviour
{
	// Laying-out the structure sequentially
	[StructLayout(LayoutKind.Sequential)]
	public struct RectCG
	{
		// Because we are laying the structure sequentially,
		// we preserve field order as they are defined.
		public Double x;
		public Double y;
		public Double width;
		public Double height;
		public RectCG(Double p1, Double p2, Double w, Double h)
		{
			x = p1;
			y = p2;
			width = w;
			height = h;
		}
	}
	//---------------------------------------------------------------------------
	//  IOS NATIVE INTERFACE
	//---------------------------------------------------------------------------
	#if UNITY_IPHONE && !UNITY_EDITOR

	[DllImport ("__Internal")]
	extern static private void MMinitWithID( string appID );
	[DllImport ("__Internal")]
	extern static private bool MMisAdReady( );
	[DllImport ("__Internal")]
	extern static private bool MMloadAd( );
	[DllImport ("__Internal")]
	extern static private bool MMshowAd( );
	[DllImport ("__Internal")]
	extern static private void MMstop( );
	[DllImport ("__Internal")]
	extern static private void MMdestroy( );
	[DllImport ("__Internal")]
	extern static private string MMgetVersion( );
	[DllImport ("__Internal")]
	extern static bool MMshowVASTAd(string url, int skipDelay, bool waterMark);
	[DllImport ("__Internal")]
	extern static private void MMfireConversion (string advertizer_id, string conversion_id);


	
	#endif // UNITY_IPHONE
	
	//---------------------------------------------------------------------------
	//  ANDROID NATIVE INTERFACE
	//---------------------------------------------------------------------------
	#if UNITY_ANDROID && !UNITY_EDITOR

	#endif // UNITY_ANDROID



//  public delegate void VideoStartedDelegate();
//
//  // DELEGATE PROPERTIES
//  public static VideoStartedDelegate          OnVideoStarted;


  //---------------------------------------------------------------------------
  //  PUBLIC INTERFACE - NON-IOS/NON-ANDROID (stub functionality)
  //---------------------------------------------------------------------------
#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
	static public void InitWithID( string app_id) {
		Debug.LogWarning( "Note: Videos can't be played in the editor." );
	}	
  	static public bool IsAdReady() { 
		return false; 
	}
	static public void LoadAd ( ) {
		Debug.LogWarning( "Load Ad only works on the device" );
	}
	static public void ShowAd ( ) {
		Debug.LogWarning( "Show Ad only works on the device" );
	}
	static public void Stop() {

	}
	static public void Destroy() {

	}
	static public string GetVersion() {
		return "Editor version";
	}
	static public bool showVASTAd(string url, int skipDelay, bool waterMark) {
		return false;
	}
	static public bool showVASTAd(string url) {
		return false;
	}
	static public bool showVASTAd(string url, bool waterMark) {
		return false;
	}
	static public bool showVASTAd(string url, int skipDelay) {
		return false;
	}
	static public void fireConversion(string advertizer_id, string conversion_id) {

	}



#endif


  //---------------------------------------------------------------------------
  //  PUBLIC INTERFACE - IOS
  //---------------------------------------------------------------------------
#if UNITY_IPHONE && !UNITY_EDITOR

	static public void InitWithID( string app_id ) {
		MMinitWithID(app_id);
	}
	static public bool IsAdReady( ) { 
		return MMisAdReady();
	}
	static public void LoadAd ( ) {
		MMloadAd();
	}
	static public void ShowAd ( ) {
		MMshowAd();
	}
	static public void Stop() {
		MMstop ();
	}
	static public void Destroy() {
		MMdestroy ();
	}
	static public string GetVersion() {
		return MMgetVersion ();
	}
	static public bool showVASTAd(string url, int skipDelay, bool waterMark) {
		return MMshowVASTAd (url, skipDelay, waterMark);
	}
	static public bool showVASTAd(string url) {
		return MMshowVASTAd (url, -1, true);
	}
	static public bool showVASTAd(string url, bool waterMark) {
		return MMshowVASTAd (url, -1, waterMark);
	}
	static public bool showVASTAd(string url, int skipDelay) {
		return MMshowVASTAd (url, skipDelay, true);
	}
	static public void fireConversion(string advertizer_id, string conversion_id) {
		MMfireConversion(advertizer_id, conversion_id);
	}
#endif

  //---------------------------------------------------------------------------
  //  PUBLIC INTERFACE - ANDROID
  //---------------------------------------------------------------------------
#if UNITY_ANDROID && !UNITY_EDITOR

	static IntPtr callbackObject;

	static private IntPtr GetUnityActivity()
	{
		// Get Unity player class.
		IntPtr unityPlayerClass = AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
		
		// Get the current activity member variable.
		IntPtr activityFieldID = AndroidJNI.GetStaticFieldID(unityPlayerClass, "currentActivity", "Landroid/app/Activity;");
		
		// Return the activity object.
		return AndroidJNI.GetStaticObjectField(unityPlayerClass, activityFieldID);
	}
	
	static public void InitWithID(string app_id)
	{
		// Get Unity's activity.
		IntPtr activityObject = GetUnityActivity();

		// Create string object for appID.
		IntPtr appIDString = AndroidJNI.NewStringUTF(app_id);

		// Prepare method parameters.
		jvalue[] args = new jvalue[2] { new jvalue() { l = activityObject }, new jvalue() { l = appIDString } };
		
		// Initialize Tremor SDK.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "initialize", "(Landroid/content/Context;Ljava/lang/String;)V");
		AndroidJNI.CallStaticVoidMethod(tremorClass, tremorMethodID, args);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}

		// Initialize callbacks.
		IntPtr callbackClass = AndroidJNI.FindClass("com/tremorvideo/adapter/TremorUnityCallback");
		IntPtr callbackMethodID = AndroidJNI.GetMethodID(callbackClass, "<init>", "()V");
		callbackObject = AndroidJNI.NewObject(callbackClass, callbackMethodID, new jvalue[0]);
	}
	
	static public bool IsAdReady()
	{
		// Return result of Tremor SDK's isAdReady.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "isAdReady", "()Z");
		bool result = AndroidJNI.CallStaticBooleanMethod(tremorClass, tremorMethodID, new jvalue[0]);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}

		return result;
	}

	static public void LoadAd()
	{
		// Call Tremor SDK's loadAd.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "loadAd", "()V");
		AndroidJNI.CallStaticVoidMethod(tremorClass, tremorMethodID, new jvalue[0]);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}
	}
	
	static public void ShowAd()
	{
		// Get Unity's activity.
		IntPtr activityObject = GetUnityActivity();

		// Prepare method parameters.
		jvalue[] args = new jvalue[2] { new jvalue() { l = activityObject }, new jvalue() { i = 0xBEEF } };

		// Call Tremor SDK's showAd.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "showAd", "(Landroid/app/Activity;I)Z");
		AndroidJNI.CallStaticBooleanMethod(tremorClass, tremorMethodID, args);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}
	}
	
	static public void Stop()
	{
		// Call Tremor SDK's stop method.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "stop", "()V");
		AndroidJNI.CallStaticVoidMethod(tremorClass, tremorMethodID, new jvalue[0]);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}
	}

	static public void Destroy()
	{
		// Call Tremor SDK's destroy method.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "destroy", "()V");
		AndroidJNI.CallStaticVoidMethod(tremorClass, tremorMethodID, new jvalue[0]);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}
	}

	static public string GetVersion()
	{
		// Get Tremor SDK version.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "getSDKVersion", "()Ljava/lang/String;");
		IntPtr versionStringObject = AndroidJNI.CallStaticObjectMethod(tremorClass, tremorMethodID, new jvalue[0]);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}

		// Return version.
		return AndroidJNI.GetStringUTFChars(versionStringObject);
	}

	static public bool showVASTAd(string url, int skipDelay, bool waterMark)
	{
		// Get Unity's activity.
		IntPtr activityObject = GetUnityActivity();

		// Create string object for url.
		IntPtr urlObject = AndroidJNI.NewStringUTF(url);
		
		// Prepare method parameters.
		jvalue[] args = new jvalue[5] { new jvalue() { l = activityObject }, new jvalue() { l = urlObject }, new jvalue() { i = 0xBEEF }, new jvalue() { i = skipDelay }, new jvalue() { z = waterMark } };
		
		// Call Tremor SDK's showVASTAd.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "showVASTAd", "(Landroid/app/Activity;Ljava/lang/String;IIZ)Z");
		bool result = AndroidJNI.CallStaticBooleanMethod(tremorClass, tremorMethodID, args);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}

		return result;
	}

	static public bool showVASTAd(string url)
	{
		return showVASTAd (url, -1, true);
	}

	static public bool showVASTAd(string url, bool waterMark)
	{
		return showVASTAd (url, -1, waterMark);
	}

	static public bool showVASTAd(string url, int skipDelay)
	{
		return showVASTAd (url, skipDelay, true);
	}

	static public void fireConversion(string advertizer_id, string conversion_id)
	{
		// Get Unity's activity.
		IntPtr activityObject = GetUnityActivity();
		
		// Create string object for advertizer_id.
		IntPtr advertizerIdObject = AndroidJNI.NewStringUTF(advertizer_id);

		// Create string object for conversion_id.
		IntPtr conversionIdObject = AndroidJNI.NewStringUTF(conversion_id);
		
		// Prepare method parameters.
		jvalue[] args = new jvalue[3] { new jvalue() { l = activityObject }, new jvalue() { l = advertizerIdObject }, new jvalue() { l = conversionIdObject } };
		
		// Call Tremor SDK's fire conversion.
		IntPtr tremorClass = AndroidJNI.FindClass("com/tremorvideo/sdk/android/videoad/TremorVideo");
		IntPtr tremorMethodID = AndroidJNI.GetStaticMethodID(tremorClass, "fireConversion", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;)Z");
		AndroidJNI.CallStaticBooleanMethod(tremorClass, tremorMethodID, args);

		// Check for exceptions
		if(AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionDescribe();
			AndroidJNI.ExceptionClear();
		}
	}

	static public void cleanUp(){
	}

#endif

	public delegate void SdkInitializedDelegate();
	public delegate void AdReadyDelegate(string success);
	public delegate void AdStartDelegate();
	public delegate void AdSkippedDelegate();
	public delegate void AdCompleteDelegate(string response);
	
	// using these delegate variables, set your desired callback functions in TVEntryPoint.cs 
	public static SdkInitializedDelegate sdkInitializedDelegate;
	public static AdReadyDelegate adReadyDelegate;
	public static AdStartDelegate adStartDelegate;
	public static AdSkippedDelegate adSkippedDelegate;
	public static AdCompleteDelegate adCompleteDelegate;

	//---------------------------------------------------------------------------
	// Delegate callbacks
	//---------------------------------------------------------------------------
	static bool configured;
	static bool was_paused;

    public static TremorVideo Instance = null;

    void Awake ()   
    {
        if (Instance == null)
        {
				Instance = this;
				// Allow UnitySendMessage to find this object.
				name = "TremorVideo";
				DontDestroyOnLoad(gameObject);
            	
        }
        else if (Instance != this)
        {
            	Destroy (gameObject);
        }
	}

	void Update() {
	}

	void sdkInitialized () {
		if (sdkInitializedDelegate != null) {
			sdkInitializedDelegate ();
		} else {
			Debug.Log ("sdkInitialized callback");
		}
	}

	void adReady (string success) {
		if (adReadyDelegate != null) {
			adReadyDelegate (success);
		} else {
			Debug.Log ("adReady callback: " + success);
		}
	}

	void adSkipped () {
		if (adSkippedDelegate != null) {
			adSkippedDelegate();
		} else {
			Debug.Log ("adSkipped callback");
		}
	}

	void adStart () {
		if (adStartDelegate != null) {
			adStartDelegate ();
		} else {
			Debug.Log ("adStart callback");
		}
	}

	void adComplete (string response) {
		if (adCompleteDelegate != null) {
			adCompleteDelegate (response);
		} else {
			Debug.Log ("adComplete callback: " + response);
		}
	}
}
