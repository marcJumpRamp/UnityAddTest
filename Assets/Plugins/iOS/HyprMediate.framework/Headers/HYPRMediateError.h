//
//  HYPRMediateError.h
//  HyprMX Mediate iOS
//
//  Created by Jeremy Ellison on 9/4/14.
//

@import Foundation;

typedef NS_ENUM(NSInteger, HYPRMediateErrorType) {
    AD_ERROR,
    REWARD_ERROR
};

extern NSString * const HYPRMediateErrorTypeString[];

@interface HYPRMediateError : NSObject

+ (instancetype)errorWithType:(HYPRMediateErrorType)errorType
                     andTitle:(NSString *)errorTitle
               andDescription:(NSString *)errorDescription;

/** The type of error
 *
 *  Error types include
 *  "showAdError" (when there's a problem with ad playback while engaging the ad provider)
 *  "rewardValidationError" (when the validation of user reward does not process properly)
 */
@property (nonatomic, readonly) HYPRMediateErrorType errorType;

/** Short description of error */
@property (nonatomic, readonly) NSString* errorTitle;

/** Detailed description of error */
@property (nonatomic, readonly) NSString* errorDescription;

@end
