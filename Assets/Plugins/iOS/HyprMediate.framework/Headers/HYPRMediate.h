//
//  HYPRMediate.h
//  HyprMX Mediate iOS
//
//  Created by Jeremy Ellison on 8/5/14.
//

@import Foundation;
@import Accelerate;
@import AdSupport;
@import AVFoundation;
@import CoreGraphics;
@import CoreLocation;
@import CoreMedia;
@import CoreFoundation;
@import MessageUI;
@import MobileCoreServices;
@import QuartzCore;
@import SystemConfiguration;
@import UIKit;
@import MediaPlayer;
@import CoreTelephony;
@import EventKit;
@import Social;
@import StoreKit;

#import "HYPRMediateReward.h"
#import "HYPRMediateError.h"
#import "HYPRMediateAdapter.h"
#import "HYPRMediateDelegate.h"

typedef void(^HYPRMediateCanShowAdCallback) (BOOL canShowAd);

typedef enum {
    HyprMediateLogLevelError = 0, // Messages at this level get logged all the time.
    HyprMediateLogLevelVerbose,   //                               ... only when verbose logging is turned on.
    HyprMediateLogLevelDebug      //                               ... in debug mode.
} HyprMediateLogLevel;

/* These methods are used by the adapters. */
/* Logs message at the HyprMediateLogLevelError level */
extern void HyprMediateLogE(NSString* log, ...);
/* Logs message at the HyprMediateLogLevelVerbose level */
extern void HyprMediateLogV(NSString* log, ...);
/* Logs message at the HyprMediateLogLevelDebug level */
extern void HyprMediateLogD(NSString* log, ...);

//! Project version number for HyprMediate iOS.
extern NSUInteger const kHYPRMediateSDKVersionNumber;

@interface HYPRMediate : NSObject <HYPRMediateAdapterDelegate>

/** Set the Mediate delegate
 *
 * @param delegate the delegate to receive callbacks from Mediate
 */
+ (void)setDelegate:(NSObject<HyprMediateDelegate>*)delegate;

/** Initialize the SDK with a given API key.
 *
 * @param mediateAPIKey the API key provided from the HyprMX Mediate Platform.
 * @param userId a unique userId identifying the user across devices.
 *
 * Should be called when your application finishes launching.
 */
+ (void)initialize:(NSString *)mediateAPIKey userId:(NSString *)userId;

/** Check whether an ad is ready to show. */
+ (void)checkInventory;

/** Show an advertisement */
+ (void)showAd;

/** Set the User ID
 *
 * @param userId a unique userId identifying the user across devices.
 * Use this method to change the user id after you've initialized the SDK
 */
+ (void)setUserId:(NSString *)userId;

/** Set the Mediate log level (Default is HyprMediateLogLevelError)
 *
 * @param logLevel how verbose the logging should be.
 */
+ (void)setLogLevel:(HyprMediateLogLevel)logLevel;

/** Fetch the Mediate log level */
+ (HyprMediateLogLevel)logLevel;

@end
