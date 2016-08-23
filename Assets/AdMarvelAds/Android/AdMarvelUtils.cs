
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
	comman static methods... 
 */
namespace AdMarvelAndroidAds
{

	public enum AdMarvelAdPosition
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public enum AdMarvelVideoEvents
	{
		IMPRESSION , START , FIRSTQUARTILE , MIDPOINT , THIRDQUARTILE , COMPLETE , CLICK , CLOSE , CUSTOM
	}
	

	public class AdMarvelSDKNetworks
	{
		
		public  const string RHYTHM = "RHYTHM" ;
		public  const string MILLENNIAL =  "MILLENNIAL" ; 
		public  const string ADMARVEL = "ADMARVEL" ;
		public  const string AMAZON = "AMAZON" ;
		public  const string ADCOLONY  ="ADCOLONY" ;
		public  const string GOOGLEPLAY  ="GOOGLEPLAY" ;
		public  const string FACEBOOK = "FACEBOOK" ;
		public  const string INMOBI = "INMOBI" ;
		public  const string HEYZAP = "HEYZAP" ;
		public  const string UNITYADS = "UNITYADS" ;
		public  const string YUME = "YUME" ;
		public  const string VUNGLE = "VUNGLE" ;
		public  const string CHARTBOOST = "CHARTBOOST" ;
	}

	internal class AdMarvelUtils 
		{

		// declared android unity plugin class with package address 

		public const string Version ="2.8.0";

		public const string UnityAdMarvelAdListenerClassName ="com.admarvel.unity.AdmarvelBanner$AdmarvelListener";
		public const string UnityAdmarvelBannerAdsClassName	="com.admarvel.unity.AdmarvelBanner";
		public const string UnityActivityClassName = "com.unity3d.player.UnityPlayer";

		public const string UnityAdMarveAdInterstitialListenerClassName	="com.admarvel.unity.AdmarvelInterstitial$AdmarvelInterstitialListener";
		public const string UnityAdmarvelInterstitialAdsClassName ="com.admarvel.unity.AdmarvelInterstitial";
		public const string UnityAdmarvelUtilsClassName ="com.admarvel.unity.Utils";

		public const string HashMapClassName = "java.util.HashMap";
		public const string StringClassName = "java.lang.String";

		private static AndroidJavaObject unityAdmarvelUtils = new AndroidJavaObject(UnityAdmarvelUtilsClassName);
		//get current activity instance.
		private static AndroidJavaClass playerClass = new AndroidJavaClass (AdMarvelUtils.UnityActivityClassName);
		private static AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");

		public static void initialize(Dictionary<string,string> publisherIds){

			// converting dictionary to java hashmap class object.
			using(AndroidJavaObject targetParams = new AndroidJavaObject(HashMapClassName))
			{
				if (publisherIds != null && publisherIds.Count > 0) {
					IntPtr method_Put = AndroidJNIHelper.GetMethodID(targetParams.GetRawClass(), "put",
					                                                 "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
					
					object[] args = new object[2];
					foreach(KeyValuePair<string, string> kvp in publisherIds)
					{
						using(AndroidJavaObject k = new AndroidJavaObject(StringClassName, kvp.Key))
						{
							using(AndroidJavaObject v = new AndroidJavaObject(StringClassName, kvp.Value))
							{
								args[0] = k;
								args[1] = v;
								AndroidJNI.CallObjectMethod(targetParams.GetRawObject(),
								                            method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
							}
						}
					}

					Debug.Log("Unity : initialize");
					
					unityAdmarvelUtils.CallStatic ("initialize",new object[2]{activity,targetParams});
				}else{
					Debug.Log("Unity : initialize - Key/value pair are not present or null");
				}
											
			}
		}


		public static void showToast(string message){
			
			unityAdmarvelUtils.CallStatic ("showMessage",new object[2]{activity,message});
			
		}

		public static AdMarvelVideoEvents getEnum( string s )
		{
			if ( AdMarvelVideoEvents.IMPRESSION.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.IMPRESSION;
			}
			else if ( AdMarvelVideoEvents.START.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.START;
			}
			else if ( AdMarvelVideoEvents.FIRSTQUARTILE.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.FIRSTQUARTILE;
			}
			else if ( AdMarvelVideoEvents.MIDPOINT.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.MIDPOINT;
			}
			else if ( AdMarvelVideoEvents.THIRDQUARTILE.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.THIRDQUARTILE;
			}
			else if ( AdMarvelVideoEvents.COMPLETE.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.COMPLETE;
			}
			else if ( AdMarvelVideoEvents.CLICK.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.CLICK;
			}
			else if ( AdMarvelVideoEvents.CLOSE.ToString().Equals( s ) )
			{
				return AdMarvelVideoEvents.CLOSE;
			}
			else
			{
				return AdMarvelVideoEvents.CUSTOM;
			}
		}

	}

	public class AdMarvelAdFailedToReceiveEventArgs : EventArgs
	{
		public string Message{ get; set; }
		
	}

	public class AdMarvelInterstitialAdFailedToReceive :EventArgs
	{
		public string Message{ get; set; }
	}
	
	
	public class AdMarvelInterstitialObjectEvent : EventArgs
	{
		public AndroidJavaObject AndroidActivity{ get; set; }
	}
	
	public class AdMarvelRewardAdResultEvent : EventArgs
	{
		public Boolean isSuccess{ get; set; }
		public string RewardName{ get; set; }
		public string RewardValue{ get; set; }
	}
	
	public class AdMarvelVideoEvent : EventArgs
	{
		public AdMarvelVideoEvents adMarvelVideoEvent{ get; set; }
		public Dictionary<string,string> customEventParams{ get; set; }
	}

}

