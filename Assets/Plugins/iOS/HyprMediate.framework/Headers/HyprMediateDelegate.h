//
//  HyprMediateDelegate.h
//  HyprMediate iOS
//
//  Created by Jeremy Ellison on 2/10/16.
//  Copyright © 2016 JunGroup Productions, LLC. All rights reserved.
//

#import "HYPRMediateReward.h"
#import "HYPRMediateError.h"

/**
 * Delegate interface for callbacks from mediation
 */
@protocol HyprMediateDelegate <NSObject>

@required

/**
 * This method will be called back when we have determined whether or not we have an add to show from the providers
 *
 * @param adCanBeDisplayed true = There is an ad available to be displayed, false = no ad available to display
 */
- (void)hyprMediateCanShowAd:(BOOL)adCanBeDisplayed;

/**
 * This method will be called back when there is a reward available.  This may be called at different times during
 * the ad display lifecycle and may be called more than once
 *
 * @param reward The reward for viewing the ad
 */
- (void)hyprMediateRewardDelivered:(HYPRMediateReward *)reward;

/**
 * This method will be called if there is an error occurs.
 *
 * @param error The HyprMediateError that occured.
 */
- (void)hyprMediateErrorOccurred:(HYPRMediateError *)error;

/**
 * The ad has started displaying
 */
- (void)hyprMediateAdStarted;

/**
 * The ad has completed displaying
 */
- (void)hyprMediateAdFinished;

@end