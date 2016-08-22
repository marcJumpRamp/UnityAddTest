//
//  HYPRMediateResult.h
//  HyprMX Mediate iOS
//
//  Created by Jeremy Ellison on 9/4/14.
//

@import Foundation;

@interface HYPRMediateReward : NSObject

/** These properties provide information about the result's virtual currency transaction */

@property (nonatomic, readonly) SInt64 virtualCurrencyAmount;
@property (nonatomic, readonly) NSString *virtualCurrencyName;

@end
