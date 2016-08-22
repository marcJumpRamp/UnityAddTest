//
//  HYPRMediateInterpreter.h
//  Unity-iPhone
//
//  Created by Ezekiel Abuhoff on 11/11/15.
//
//

#import <Foundation/Foundation.h>

@interface HYPRMediateInterpreter : NSObject

+ (void)initializeHyprMediateWithAPIToken: (NSString *)apiToken
                                   userID: (NSString *)userID
                               customInfo: (NSString *)customInfo;

+ (void)checkInventory;

+ (void)showAd;

+ (void)setUserID: (NSString *)userID;

+ (void)setLogLevel: (NSUInteger)logLevel;

+ (NSUInteger)logLevel;

+ (NSUInteger)hyprMediateSDKVersionNumber;

@end
