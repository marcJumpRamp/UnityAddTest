//
//  HYPRMediateInterpreter.m
//  Unity-iPhone
//
//  Created by Ezekiel Abuhoff on 11/11/15.
//
//

#import "HYPRMediateInterpreter.h"
#import "HyprMediateUnityInterface.mm"
#import <HyprMediate/HyprMediate.h>

const NSString *kUnityHyprMediateObjectName = @"HyprMediate";
const NSString *kUnityInventoryCallbackName = @"NativeInventoryCallback";
const NSString *kUnityRewardCallbackName = @"NativeRewardCallback";
const NSString *kUnityErrorCallbackName = @"NativeErrorCallback";
const NSString *kUnityAdStartedCallbackName = @"NativeAdStartedCallback";
const NSString *kUnityAdFinishedCallbackName = @"NativeAdFinishedCallback";

@interface HYPRMediate ()

+ (void)initialize:(NSString *)mediateAPIKey
            userId:(NSString *)userId
        customInfo:(NSString *)customInfo;

@end

@interface HYPRMediateInterpreter () <HyprMediateDelegate>

@end

HYPRMediateInterpreter *HYPRMediateSharedInterpreter = nil;

@implementation HYPRMediateInterpreter

#pragma mark - Singleton

+ (instancetype)sharedInstance {
    static dispatch_once_t token;
    dispatch_once(&token, ^{
        HYPRMediateSharedInterpreter = [HYPRMediateInterpreter new];
    });
    return HYPRMediateSharedInterpreter;
}

#pragma mark - Public Methods

+ (void)initializeHyprMediateWithAPIToken: (NSString *)apiToken
                                   userID: (NSString *)userID
                               customInfo: (NSString *)customInfo {
    [[HYPRMediateInterpreter sharedInstance] initializeHyprMediateWithAPIToken: apiToken
                                                                        userID: userID
                                                                    customInfo: customInfo];
}

- (void)initializeHyprMediateWithAPIToken: (NSString *)apiToken
                                   userID: (NSString *)userID
                               customInfo: (NSString *)customInfo {
    [HYPRMediate initialize:apiToken userId:userID customInfo:customInfo];
    [HYPRMediate setDelegate: (id <HyprMediateDelegate>)self];
}

+ (void)checkInventory {
    [HYPRMediate checkInventory];
}

+ (void)showAd {
    [HYPRMediate showAd];
}

+ (void)setUserID: (NSString *)userID {
    [HYPRMediate setUserId:userID];
}

+ (void)setLogLevel: (NSUInteger)logLevel {
    [HYPRMediate setLogLevel:(HyprMediateLogLevel)logLevel];
}

+ (NSUInteger)logLevel {
    return [HYPRMediate logLevel];
}

+ (NSUInteger)hyprMediateSDKVersionNumber {
    return kHYPRMediateSDKVersionNumber;
}

#pragma mark - Delegate Methods

- (void)hyprMediateCanShowAd:(BOOL)adCanBeDisplayed {
    NSString *availability = adCanBeDisplayed ? @"true" : @"false";
    [HYPRMediateInterpreter checkInventoryCallbackWithString:availability];
}

- (void)hyprMediateRewardDelivered:(HYPRMediateReward *)reward {
    NSString *rewardString = [HYPRMediateInterpreter stringFromHyprMediateReward:reward];
    [HYPRMediateInterpreter rewardCallbackWithString:rewardString];
}

- (void)hyprMediateErrorOccurred:(HYPRMediateError *)error {
    NSString *errorString = [HYPRMediateInterpreter stringFromHyprMediateError:error];
    [HYPRMediateInterpreter errorCallbackWithString:errorString];
}

- (void)hyprMediateAdStarted {
    [HYPRMediateInterpreter adStartedCallback];
}

- (void)hyprMediateAdFinished {
    [HYPRMediateInterpreter adFinishedCallback];
}

#pragma mark - Utility Methods

+ (NSString *)stringFromHyprMediateReward: (HYPRMediateReward *)reward {
    NSDictionary *rewardDictionary = @{@"virtualCurrencyAmount": @(reward.virtualCurrencyAmount),
                                       @"virtualCurrencyName": reward.virtualCurrencyName};
    NSData *rewardData = [NSJSONSerialization dataWithJSONObject:rewardDictionary
                                                         options:NSJSONWritingPrettyPrinted
                                                           error:nil];
    return [[NSString alloc] initWithData:rewardData encoding:NSUTF8StringEncoding];
}

+ (NSString *)stringFromHyprMediateError: (HYPRMediateError *)error {
    NSString *errorTypeString = HYPRMediateErrorTypeString[error.errorType];
    NSDictionary *errorDictionary = @{@"errorType": errorTypeString,
                                      @"errorTitle": error.errorTitle,
                                      @"errorDescription": error.errorDescription};
    NSData *errorData = [NSJSONSerialization dataWithJSONObject:errorDictionary
                                                        options:NSJSONWritingPrettyPrinted
                                                          error:nil];
    return [[NSString alloc] initWithData:errorData encoding:NSUTF8StringEncoding];
}

#pragma mark - Sending Messages Back To Unity

+ (void)checkInventoryCallbackWithString: (NSString *)inventoryString {
    UnitySendMessage([kUnityHyprMediateObjectName UTF8String],
                     [kUnityInventoryCallbackName UTF8String],
                     [inventoryString UTF8String]);
}

+ (void)rewardCallbackWithString: (NSString *)rewardString {
    UnitySendMessage([kUnityHyprMediateObjectName UTF8String],
                     [kUnityRewardCallbackName UTF8String],
                     [rewardString UTF8String]);
}

+ (void)errorCallbackWithString: (NSString *)errorString {
    UnitySendMessage([kUnityHyprMediateObjectName UTF8String],
                     [kUnityErrorCallbackName UTF8String],
                     [errorString UTF8String]);
}

+ (void)adStartedCallback {
    UnitySendMessage([kUnityHyprMediateObjectName UTF8String],
                     [kUnityAdStartedCallbackName UTF8String],
                     [@"" UTF8String]);
}

+ (void)adFinishedCallback {
    UnitySendMessage([kUnityHyprMediateObjectName UTF8String],
                     [kUnityAdFinishedCallbackName UTF8String],
                     [@"" UTF8String]);
}

@end
