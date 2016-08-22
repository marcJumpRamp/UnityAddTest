//
//  HYPRMediateAdapter.h
//  HyprMX Mediate iOS
//
//  Created by Jeremy Ellison on 8/5/14.
//

#import <Foundation/Foundation.h>

@class HYPRMediateReward;

typedef void(^HYPRMediateCanShowAdCallback) (BOOL canShowAd);
typedef void(^HYPRMediateAdShownCallback) (HYPRMediateReward *result);

@protocol HYPRMediateAdapterDelegate;

@class HYPRMediateReward;
@class HYPRMediateError;

@protocol HYPRMediateAdapter <NSObject>

/** This method may be called multiple times.
 *
 * @discussion It is the adapter's responsibility to maintain ad provider class instances and apply new SDK settings.
 */
- (id)initWithConfiguration:(NSDictionary *)configuration
                     userId:(NSString *)userId
                   delegate:(id<HYPRMediateAdapterDelegate>)delegate;

/** Calls the callback with the availability of an ad.
 *
 * @param callback the callback to call, takes a boolean representing the availability.
 *
 * @discussion This may be Asynchronous, or call the callback immediately. If it is async, the
 *  mediation SDK will time it out if it takes too long and call callback(NO) to prevent apps
 *  from hanging.
 */
- (void)canShowAd:(HYPRMediateCanShowAdCallback)callback;

/** Begins ad playback if an ad is available.
 *
 * @discussion Do not call this method while an ad is playing. */
- (void)showAd;

/** Call this method to change the current user id.
 *
 * @param userId the user id to set.
 */
- (void)setUserId:(NSString *)userId;

/** Returns the adapter's version
 *
 * @returns the adapter's version as an unsigned integer.
 */
- (NSUInteger)version;

/** Returns the ad provider sdk's version.
 */
- (NSString *)adProviderSdkVersion;

/** Returns the version the adapter expects the SDK to be.
 *
 * @returns the expected Mediation SDK version, as an unsigned integer.
 */
- (NSUInteger)requiredAdapterAPIVersion;

@end

@protocol HYPRMediateAdapterDelegate <NSObject>

@required

/** This event will occur if an adapter fails to validate its configuration settings
 *
 * @param adapter the adapter triggering the event
 * @param error a string describing what went wrong.
 */
 - (void)mediateAdapter:(id<HYPRMediateAdapter>)adapter didFailToInitializeWithError:(NSString *)error;

/** This event will occur when an ad provider takes over the screen
 *
 * @param adapter the adapter triggering the event
 */
- (void)mediateAdapterStartedDisplaying:(id<HYPRMediateAdapter>)adapter;

/** This event will occur when an ad provider relinquishes the screen
 *
 * @param adapter the adapter triggering the event
 */
- (void)mediateAdapterFinishedDisplaying:(id<HYPRMediateAdapter>)adapter;

/** This event will occur when an ad provider delivers a reward
 *
 * @param adapter the adapter triggering the event
 * @param reward a HYPRMediateReward
 */
- (void)mediateAdapterDeliveredReward:(id<HYPRMediateAdapter>)adapter;

/** This event can occur when an ad provider fails to deliver a reward
 *
 * @param adapter the adapter triggering the event
 * @param error a HYPRMediateError
 */
- (void)mediateAdapter:(id<HYPRMediateAdapter>)adapter errorOccurred:(HYPRMediateError *)error;

/** This event will be triggered if an adapter is told to show an ad but fails to.
 *
 * @param adapter the adapter triggering the event
 * @param error a HYPRMediateError
 */
- (void)mediateAdapter:(id<HYPRMediateAdapter>)adapter adFailedToDisplay:(HYPRMediateError *)error;

@end