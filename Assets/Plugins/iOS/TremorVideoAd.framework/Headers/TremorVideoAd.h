//
//  TremorVideoAd.h
//  TremorVideoAd
//
//  Created by Heather Stark on 11/5/15.
//  Copyright © 2015 Tremor Video Inc. All rights reserved.
//

#import <UIKit/UIKit.h>

typedef enum {
    ORIENTATION_ANY = 0,
    ORIENTATION_LANDSCAPE,
    ORIENTATION_PORTRAIT
}ePreferredOrientation;

typedef enum  {
    GENDER_UNKNOWN = 0,
    GENDER_MALE,
    GENDER_FEMALE,
    GENDER_LAST
}eUserGender;

typedef enum  {
    EDUCATION_UNKNOWN = 0,
    EDUCATION_LESS_THAN_HIGHSCHOOL,
    EDUCATION_SOME_HIGHSCHOOL,
    EDUCATION_HIGHSCHOOL,
    EDUCATION_SOME_COLLEGE,
    EDUCATION_COLLEGE_BACHELOR,
    EDUCATION_COLLEGE_MASTERS,
    EDUCATION_COLLEGE_PROFESSIONAL,
    EDUCATION_COLLEGE_PHD,
    EDUCATION_LAST
}eUserEducation;

typedef enum  {
    RACE_UNKNOWN = 0,
    RACE_ASIAN,
    RACE_WHITE,
    RACE_BLACK,
    RACE_HISPANIC,
    RACE_AMERICAN_INDIAN,
    RACE_ALASKA_NATIVE,
    RACE_NATIVE_HAWAIIAN,
    RACE_PACIFIC_ISLANDER,
    RACE_OTHER
}eUserRace;

typedef enum  {
    INCOME_UNKNOWN = 0,
    INCOME_LESS_THAN_25K,
    INCOME_25K_50K,
    INCOME_50K_75K,
    INCOME_75K_100K,
    INCOME_100K_150K,
    INCOME_150K_200K,
    INCOME_200K_250K,
    INCOME_ABOVE_250K
}eIncome;

typedef enum {
    kTremorResponseCodeError    = -1,
    kTremorResponseCodeNoAd     = 0,
    kTremorResponseCodeSuccess  = 1
}eTremorResponseCode;


@interface TremorVideoSettings : NSObject {
    
}

@property (nonatomic) ePreferredOrientation preferredOrientation;
@property (nonatomic, retain) NSArray *category;
@property (nonatomic) eUserGender userGender;
@property (nonatomic) eUserEducation userEducation;
@property (nonatomic) eUserRace userRace;
@property (nonatomic) eIncome userIncomeRange;
@property (nonatomic) NSUInteger userAge;
@property (nonatomic, retain) NSString *userZip;
@property (nonatomic) double userLatitude;
@property (nonatomic) double userLongitude;
@property (nonatomic, retain) NSString *userLanguage;
@property (nonatomic, retain) NSString *userCountry;
@property (nonatomic, retain) NSArray *userInterests;
@property (nonatomic, retain) NSDictionary *misc;
@property (nonatomic) NSUInteger maxAdTimeSeconds;
@property (nonatomic, retain) NSString *policyID;
@property (nonatomic, retain) NSArray *adBlocks;
@property (nonatomic, retain) NSString *contentTitle;
@property (nonatomic, retain) NSString *contentDescription;
@property (nonatomic, retain) NSString *contentID;

@end


@protocol TremorVideoAdDelegate <NSObject>
@optional
/*
 * If Ads are being pre-fetched
 * this method will be called when pre-fetching ad is complete
 *
 * Parameters
 * success -
 *      true: ad pre-fetched successfully
 *      false: an error occured during the pre-fetching
 */
- (void)adReady:(BOOL)success;

/*
 * This method will be called when an ad is fully loaded
 * and about to start rendering on screen
 */
- (void)adStart;

/*
 * This method will be called when an the Tremor SDK has finished with
 * the request to show an Ad.
 *
 * Parameters
 * sucess -
 *      true: ad showed successfully
 *      false: ad was not shown or an error occurred
 *
 * responseCode -
 *      kTremorResponseCodeSuccess:
 *          ad showed succesfully
 *      kTremorResponseCodeNoAd:
 *          No ad was available at the time
 *      kTremorResponseCodeError:
 *          An ad was returned but an error occured tyring to show it
 */
- (void)adComplete:(BOOL)success responseCode:(NSInteger)responseCode;

/*
 * This method will be called when the user skips the ad
 */
- (void)adSkipped;

/*
 * This method will be called when the user clicks on the ad and open a webpage
 */
- (void)adClickThru;

/*
 * This method will be called when an ad video impression event is recorded
 */
- (void)adImpression;

/*
 * This method will be called when an ad video starts
 *
 * Parameters
 * videoId -
 *      Name of video ID.
 *      An ad might have multiple videos. The name of first video is "video-1".
 */
- (void)adVideoStart:(NSString *) videoId;

/*
 * This method will be called when an ad video hits the first quartile
 *
 * Parameters
 * videoId -
 *      Name of video ID.
 *      An ad might have multiple videos. The name of first video is "video-1".
 */
- (void)adVideoFirstQuartile:(NSString *) videoId;

/*
 * This method will be called when an ad video hits midpoint
 *
 * Parameters
 * videoId -
 *      Name of video ID.
 *      An ad might have multiple videos. The name of first video is "video-1".
 */
- (void)adVideoMidPoint:(NSString *) videoId;

/*
 * This method will be called when an ad video hits the third quartile
 *
 * Parameters
 * videoId -
 *      Name of video ID.
 *      An ad might have multiple videos. The name of first video is "video-1".
 */
- (void)adVideoThirdQuartile:(NSString *) videoId;

/*
 * This method will be called when an ad video ends
 *
 * Parameters
 * videoId -
 *      Name of video ID.
 *      An ad might have multiple videos. The name of first video is "video-1".
 */
- (void)adVideoComplete:(NSString *) videoId;
@end


@interface TremorVideoAd : NSObject {
    
}

/*
 * This method is used to initialize the TremorVideo static instance.
 * It requires that you pass your custom appID.  It is recommended that this is called at your application launch.
 
 * Parameters
 * appID -
 *      Your application’s unique appID.
 */
+ (void)initWithAppID:(NSString *)appID;

/*
 * Use the this method to pre-fetch a video ad.
 * This method communicates over the network to request ads and download them in the background.
 * You must call this method if you wish to show any ads.
 */
+ (void)loadAd;

/*
 * This method attempts to show an ad on your behalf.
 *
 * Parameters
 * parentViewController -
 *      This UIViewController should be the UIViewController at the top of hierarchy of current Window.
 *
 * Return Value
 *      true if the Tremor SDK displayed to screen.
 *      Otherwise false. An unsuccessful attempt to display the ad may be the result of a no ad match,
 *      a download in progress, or an error.
 */
+ (BOOL)showAd:(UIViewController *)parentViewController;

/*
 * This method is used to suspend the ad request and download process.
 * This method works asynchronously to suspend the background thread. The results of calling stop may not
 * take effect immediately. Use this method to stop the TremorVideo SDK from using any CPU and network resources.
 */
+ (void)stop;

/*
 * This meothod is used to shutdown any background processes and to perform cleanup of the SDK.  */
+ (void)destroy;

/*
 * This method is used to check if a video ad is ready to be shown.
 * (If you show a video ad in a frame, use “isAdReadyForFrame”.
 *
 * Return Value
 *      true if a video ad is ready to be shown. Otherwise false.
 */
+ (BOOL)isAdReady;
/*
 * This method returns a TremorVideoSettings object.
 * You can set properties of this object to update targeting information
 * and other custom settings you’d like to use.
 *
 * Return Value
 *      A pointer to the TremorVideoSettings object.
 */
+ (TremorVideoSettings *)getSettings;

/*
 * Set a delegate to track changes of Ad state
 */
+ (void)setDelegate:(id<TremorVideoAdDelegate>)delegate;

/*
 * Return Value
 *      String value of the current Tremor SDK version
 */
+ (NSString *)getVersion;

/*
 * Returns a pointer to the UIView which the ad is displayed in.
 * You must retrieve this UIView to add it as a subview to your application’s view
 * when using showAd:withFrame.
 *
 * Return Value
 *      A pointer to the UIView which the ad is displayed in.
 */
+ (BOOL)showAd:(UIViewController *)parentViewController withFrame:(CGRect)frame;

/*
 * Returns a pointer to the UIView which the ad is displayed in. You must retrieve this UIView
 * to add it as a subview to your application’s view when using showAd:withFrame.
 *
 * Return Value
 *      A pointer to the UIView which the ad is displayed in.
 */
+ (UIView *)adView;
/*
 * Changes the bounds that the video is displayed in.
 * This can be called while the ad is playing and is useful if the bounds of the video must change
 * during screen rotations. This method can only be used when showing an ad with showAd:withFrame.
 
 * Parameters
 * frame -
 *      The frame that the video ad should display within.
 */
+ (void)setFrame:(CGRect)frame;

/*
 * "isAdReadyForFrame” checks if a video ad is ready to be shown in a frame.
 *
 * Return Value
 * true -
 *      if a video ad is ready to be shown in a frame. Otherwise false.
 */
+ (BOOL)isAdReadyForFrame;

/*
 * "stopAd" is used to stop the ad in frame. Use this method to stop a video ad shown within a frame.
 */
+ (void)stopAd;

/*
 * APIs for VAST Display
 */
+ (BOOL)showVASTAd:(UIViewController *)parentViewController vastURL:(NSString *)url;
+ (BOOL)showVASTAd:(UIViewController *)parentViewController vastURL:(NSString *)url skipDelay:(int)skipDelay;
+ (BOOL)showVASTAd:(UIViewController *)parentViewController vastURL:(NSString *)url waterMark:(BOOL)waterMark;
+ (BOOL)showVASTAd:(UIViewController *)parentViewController vastURL:(NSString *)url skipDelay:(int)skipDelay waterMark:(BOOL)waterMark;

/*
 * APIs for App Analytics
 */
+ (void)handleAnalyticsEvent:(NSString *)event;
+ (void)handleAnalyticsEvent:(NSString *)event parameters:(NSDictionary *)parameters;
+ (void)handleAnalyticsStateChange:(NSString *)newState;

/*
 * APIs for Conversion Tracking
 */
+ (void)fireConversion:(NSString *)advertizer_id :(NSString *)conversion_id;


@end

